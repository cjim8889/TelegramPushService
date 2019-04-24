using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramPushService.Context;
using TelegramPushService.Models;

namespace TelegramPushService.Services
{
    public class MqService
    {
        private readonly MqContext mqContext;
        public MqService(MqContext mqContext)
        {
            this.mqContext = mqContext;
        }

        public async Task PushMessage(string message, List<int> receiver)
        {

            var messageJson = JsonConvert.SerializeObject(new MqMessage { Content = message, Receivers = receiver });
            var body = Encoding.UTF8.GetBytes(messageJson);

            var task = Task.Run(() => {
                mqContext.Channel.BasicPublish(exchange: "",
                    routingKey: "test",
                    basicProperties: null,
                    body: body);
                });

            Console.WriteLine(messageJson);

            await task;
        }

        public async Task PushMessage(string message, int receiver)
        {
            await PushMessage(message, new List<int> { receiver });
        }

        public async Task PushChallengeMessage(string challengeCode, int receiver)
        {
            await PushChallengeMessage(challengeCode, new List<int> { receiver });
        }

        public async Task PushChallengeMessage(string challengeCode, List<int> receiver)
        {

            string message = $"Challenge Code: {challengeCode}\n";

            await PushMessage(message, receiver);
        }

    }
}
