using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TelegramPushService.Models;

namespace TelegramPushService.Context
{
    public class DatabaseContext
    {
        public DatabaseContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDb:ConnectionString").Value;
            var database = configuration.GetSection("MongoDb:Database").Value;

            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(database);

        }

        private IMongoDatabase Database { get; }
        private IMongoClient Client { get; }

        public IMongoCollection<Publisher> Publishers => Database.GetCollection<Publisher>("Publishers");



    }
}
