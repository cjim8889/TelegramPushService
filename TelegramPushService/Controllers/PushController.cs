using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelegramPushService.Services;

namespace TelegramPushService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushController : ControllerBase
    {
        private readonly DatabaseService databaseService;
        private readonly MqService mqService;
        public PushController(DatabaseService databaseService, MqService mqService)
        {
            this.databaseService = databaseService;
            this.mqService = mqService;
        }


        [HttpGet]

        public async Task<ActionResult> PushMessage([FromQuery(Name = "p")] string publisherToken, [FromQuery(Name = "m")] string message)
        {
            var publisher = await databaseService.GetPublisherByTokenAsync(publisherToken);

            if (publisher == null)
            {
                return BadRequest();
            }

            await mqService.PushMessage(message, publisher.Subscribers);

            return Accepted(new { message="Success" });
        }
    }
}