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

        public async Task<Publisher> GetPublisherByPushTokenAsync(string pushToken)
        {
            return await databaseContext.Publishers.Find(publisher => publisher.PushToken == pushToken).FirstOrDefaultAsync();
        }

        public async Task<Publisher> GetPublisherByAdminTokenAsync(string adminToken)
        {
            return await databaseContext.Publishers.Find(publisher => publisher.AdminToken == adminToken).FirstOrDefaultAsync();
        }

        public async Task<UpdateResult> AddNewSubscriberAsync(string adminToken, int subsriberId)
        {
            var filter = Builders<Publisher>.Filter.Eq("AdminToken", adminToken);
            var update = Builders<Publisher>.Update.AddToSet("Subscribers", subsriberId);

            return await databaseContext.Publishers.UpdateOneAsync(filter, update);
        }

        public async Task<bool> IsAdminTokenValid(string adminToken)
        {
            return await GetPublisherByAdminTokenAsync(adminToken) != null;
        }

        public async Task RemoveSubsriberAsync(Publisher publisher, int subsriberId)
        {
            var filter = Builders<Publisher>.Filter.Eq("Id", publisher.Id);

            publisher.Subscribers.Remove(subsriberId);

            await databaseContext.Publishers.ReplaceOneAsync(filter, publisher);
        }

        public async Task RemoveSubsriberAsync(string adminToken, int subsriberId)
        {
            var publisher = await GetPublisherByAdminTokenAsync(adminToken);
            if (publisher != null)
            {
                await RemoveSubsriberAsync(publisher, subsriberId);
            }
        }

        public async Task SetValidationStatusAsync(string adminToken, bool status)
        {
            var filter = Builders<Publisher>.Filter.Eq("AdminToken", adminToken);
            var update = Builders<Publisher>.Update.Set("Validated", status);

            await databaseContext.Publishers.UpdateOneAsync(filter, update);
        }


    }
}
