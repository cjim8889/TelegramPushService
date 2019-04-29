using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;

namespace TelegramPushService.Models
{
    public class Publisher
    {

        public Publisher()
        {
            Validated = false;
            Subscribers = new List<int>();

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] adminTokenData = new byte[32];
                byte[] pushTokenData = new byte[32];

                rng.GetBytes(adminTokenData);
                rng.GetBytes(pushTokenData);

                AdminToken = Convert.ToBase64String(adminTokenData);
                PushToken = Convert.ToBase64String(pushTokenData);
            }
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("PushToken")]
        public string PushToken { get; set; }

        [BsonElement("AdminToken")]
        public string AdminToken { get; set; }

        [BsonElement("Subscribers")]
        public List<int> Subscribers { get; set; }

        [BsonElement("Validated")]
        public bool Validated { get; set; }
    }
}
