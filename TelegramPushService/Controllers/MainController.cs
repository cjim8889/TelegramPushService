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

        [HttpGet("insert")]
        public async Task<ActionResult> InsertSubscriber(string publisherToken, int subsriberId)
        {
            var result = await databaseService.AddNewSubscriberAsync(publisherToken, subsriberId);

            return CreatedAtAction("InsertSubscriber", new { IsAcknowledged=result.IsAcknowledged });
        }

        [HttpGet("authenticate")]
        public async Task<ActionResult> Authenticate(string publisherToken, int subsriberId)
        {
            var publisher = await databaseService.GetPublisherByTokenAsync(publisherToken);

            if (publisher == null)
            {
                return NotFound();
            }

            var challengeCode = authService.GenerateChallengeCode();
            authService.AddAuth(publisher.Id, challengeCode);
            await mqService.PushChallengeMessage(challengeCode, subsriberId);

            return Accepted(new { message = "Challenge Code Sent" });
        }

        [HttpGet("validate")]

        public async Task<ActionResult> Validate(string publisherToken, string challengeCode)
        {
            var publisher = await databaseService.GetPublisherByTokenAsync(publisherToken);

            if (publisher == null)
            {
                return NotFound();
            }

            var state = authService.Authenticate(publisher.Id, challengeCode);

            if (state)
            {
                await databaseService.SetValidationStatusAsync(publisher.Id, true);
                return Accepted(new { message = "Validated" });
            }

            return BadRequest();
        }
    }
}