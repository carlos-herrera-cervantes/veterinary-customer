using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace VeterinaryCustomer.Repositories.Repositories;

public class AddressRepository : IAddressRepository
{
    #region snippet_Properties

    private readonly IMongoCollection<Address> _collection;

    #endregion

    #region snippet_Constructors

    public AddressRepository(IMongoClient mongoClient)
    {
        var database = Environment.GetEnvironmentVariable("MONGODB_DEFAULT_DB");
        _collection = mongoClient.GetDatabase(database).GetCollection<Address>("addresses");
    }

    #endregion

    #region snippet_ActionMethods

    public async Task<int> CountAsync(FilterDefinition<Address> filterDefinition)
        => (int)await _collection.CountDocumentsAsync(filterDefinition);

    public async Task<Address> GetByCustomerIdAsync(string customerId)
    {
        var filter = Builders<Address>.Filter.Eq(a => a.CustomerId, customerId);
        return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Address address) => await _collection.InsertOneAsync(address);

    public async Task UpdateAsync(Address address, JsonPatchDocument<Address> patchDocument)
    {
        patchDocument.ApplyTo(address);
        address.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<Address>.Filter.Eq(a => a.CustomerId, address.CustomerId);

        await _collection.ReplaceOneAsync(filter, address);
    }

    public async Task DeleteManyAsync(FilterDefinition<Address> filter)
        => await _collection.DeleteManyAsync(filter);

    #endregion
}
