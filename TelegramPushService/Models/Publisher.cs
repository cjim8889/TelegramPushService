using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelegramPushService.Models
{
    public class Publisher
    {

        public Publisher()
        {
            Validated = false;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Token")]
        public string Token { get; set; }


        [BsonElement("Subscribers")]
        public List<int> Subscribers { get; set; }

        [BsonElement("Validated")]
        public bool Validated { get; set; }
    }
}
