using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TelegramPushService.Context;
using TelegramPushService.Models;

namespace TelegramPushService.Services
{
    public class DatabaseService
    {
        private readonly DatabaseContext databaseContext;

        public DatabaseService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<List<Publisher>> GetPublishersAsync()
        {
            return await databaseContext.Publishers.Find(publisher => true).ToListAsync();
        }

        public async Task CreatePublisherAsync(Publisher publisher)
        {
            await databaseContext.Publishers.InsertOneAsync(publisher);
        }

        public async Task<Publisher> GetPublisherByIdAsync(string id)
        {
           return await databaseContext.Publishers.Find(publisher => publisher.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Publisher> GetPublisherByTokenAsync(string token)
        {
            return await databaseContext.Publishers.Find(publisher => publisher.Token == token).FirstOrDefaultAsync();
        }

        public async Task<UpdateResult> AddNewSubscriberAsync(string publisherToken, int subsriberId)
        {
            var filter = Builders<Publisher>.Filter.Eq("Token", publisherToken);
            var update = Builders<Publisher>.Update.Push<int>("Subscribers", subsriberId);

            return await databaseContext.Publishers.UpdateOneAsync(filter, update);
        }

        public async Task SetValidationStatusAsync(string publisherId, bool status)
        {
            var filter = Builders<Publisher>.Filter.Eq("Id", publisherId);
            var update = Builders<Publisher>.Update.Set("Validated", status);

            await databaseContext.Publishers.UpdateOneAsync(filter, update);
        }


    }
}
