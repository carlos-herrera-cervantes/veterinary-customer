using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;

namespace VeterinaryCustomer.Repositories.Repositories;

public class ProfileRepository : IProfileRepository
{
    #region snippet_Properties

    private readonly IMongoCollection<Profile> _collection;

    #endregion

    #region snippet_Constructors

    public ProfileRepository(IMongoClient mongoClient)
    {
        var database = Environment.GetEnvironmentVariable("MONGODB_DEFAULT_DB");
        _collection = mongoClient.GetDatabase(database).GetCollection<Profile>("profiles");
    }

    #endregion

    #region snippet_ActionMethods

    public async Task<int> CountAsync(FilterDefinition<Profile> filterDefinition)
        => (int)await _collection.CountDocumentsAsync(filterDefinition);

    public async Task<IEnumerable<Profile>> GetAllAsync(int page, int pageSize)
        => await _collection
            .Find(Builders<Profile>.Filter.Empty)
            .Skip(page * pageSize)
            .Limit(pageSize)
            .ToListAsync();

    public async Task<Profile> GetByCustomerIdAsync(string customerId)
    {
        var filter = Builders<Profile>.Filter.Eq(p => p.CustomerId, customerId);
        return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Profile profile)
    {
        profile.UpdatedAt = DateTime.UtcNow;
        var filter = Builders<Profile>.Filter.Eq(p => p.CustomerId, profile.CustomerId);
        await _collection.ReplaceOneAsync(filter, profile);
    }

    #endregion
}
