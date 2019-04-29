using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelegramPushService.Models;
using TelegramPushService.Services;

namespace TelegramPushService.Controllers
{
    [Route("api/publisher")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly DatabaseService databaseService;
        private readonly MqService mqService;
        private readonly AuthService authService;
        public MainController(DatabaseService databaseService, MqService mqService, AuthService authService)
        {
            this.databaseService = databaseService;
            this.mqService = mqService;
            this.authService = authService;
        }


        [HttpGet]
        public async Task<ActionResult<List<Publisher>>> GetPublishers()
        {
            return await databaseService.GetPublishersAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Publisher>> CreatePublisher(Publisher publisher)
        {
            await databaseService.CreatePublisherAsync(publisher);
            return publisher;
        }

        [HttpGet("subsriber/insert")]
        public async Task<ActionResult> InsertSubsriber(string adminToken, int subsriberId)
        {
            if (await databaseService.IsAdminTokenValid(adminToken))
            {
                var challengeCode = authService.GenerateChallengeCode();
                authService.AddAuth(adminToken, challengeCode, subsriberId);
                await mqService.PushChallengeMessage(challengeCode, subsriberId);

                return Accepted(new { message = "Challenge Code Sent" });
            }

            return NotFound();
            
        }

        [HttpGet("subsriber/validate")]

        public async Task<ActionResult> Validate(string adminToken, string challengeCode)
        {
            if (!authService.ContainsPublisher(adminToken))
            {
                return NotFound();
            }

            if (authService.Authenticate(adminToken, challengeCode))
            {

                var subscriberId = authService.GetSubsriberId(adminToken);

                await databaseService.AddNewSubscriberAsync(adminToken, subscriberId);

                authService.Remove(adminToken);

                return Accepted(new { message = "Validated", subscriberId });
            }

            return NotFound();
        }

        [HttpGet("subsriber/remove")]
        public async Task<ActionResult> RemoveSubsriber(string adminToken, int subsriberId)
        {
            await databaseService.RemoveSubsriberAsync(adminToken, subsriberId);
            return Accepted();
        }

    }
}