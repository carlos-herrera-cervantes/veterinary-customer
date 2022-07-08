using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;

namespace VeterinaryCustomer.Repositories.Repositories
{
    public class AvatarRepository : IAvatarRepository
    {
        #region snippet_Properties

        private readonly IMongoCollection<Avatar> _collection;

        #endregion

        #region snippet_Constructors

        public AvatarRepository(IMongoClient mongoClient)
        {
            var database = Environment.GetEnvironmentVariable("MONGODB_DEFAULT_DB");
            _collection = mongoClient.GetDatabase(database).GetCollection<Avatar>("avatars");
        }

        #endregion

        #region snippet_ActionMethods

        public async Task<int> CountAsync(FilterDefinition<Avatar> filterDefinition)
            => (int)await _collection.CountDocumentsAsync(filterDefinition);

        public async Task<Avatar> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Avatar>.Filter.Eq("customer_id", customerId);
            return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Avatar avatar) => await _collection.InsertOneAsync(avatar);

        public async Task UpdateAsync(Avatar avatar)
        {
            var filter = Builders<Avatar>.Filter.Eq("customer_id", avatar.CustomerId);
            await _collection.ReplaceOneAsync(filter, avatar);
        }

        public async Task DeleteAsync(string customerId)
        {
            var filter = Builders<Avatar>.Filter.Eq("customer_id", customerId);
            await _collection.DeleteOneAsync(filter);    
        }

        #endregion
    }
}
