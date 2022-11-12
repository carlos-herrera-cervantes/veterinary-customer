using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories;

[Collection("AddressRepositoryIntegration")]
public class AddressRepositoryIntegrationTests
{
    #region snippet_Properties

    private readonly IMongoClient _mongoClient;

    #endregion

    #region snippet_Constructors

    public AddressRepositoryIntegrationTests()
    {
        string uri = Environment.GetEnvironmentVariable("MONGODB_URI");
        _mongoClient = new MongoClient(uri);
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 0 documents")]
    public async Task CountAsyncShouldReturnZeroDocuments()
    {
        var addressRepository = new AddressRepository(_mongoClient);
        var counter = await addressRepository.CountAsync(Builders<Address>.Filter.Empty);
        Assert.Equal<int>(0, counter);
    }

    [Fact(DisplayName = "Should return null when document does not exist")]
    public async Task GetByCustomerIdAsyncShouldReturnNull()
    {
        var addressRepository = new AddressRepository(_mongoClient);
        var address = await addressRepository.GetByCustomerIdAsync("636fe443ddb217dce0b934f0");
        Assert.Null(address);
    }

    [Fact(DisplayName = "Should create and update a document")]
    public async Task CreateAndUpdateAsyncShouldCreateAndUpdateDocument()
    {
        var newAddress = new Address
        {
            CustomerId = "636fe7c0ed55f012b0a17ebc",
            City = "Orizaba",
            PostalCode = "39330",
            Street = "Calle falsa 123",
            Colony = "Bella Vista",
            Number = "23"
        };
        var addressRepository = new AddressRepository(_mongoClient);

        await addressRepository.CreateAsync(newAddress);

        var counter = await addressRepository.CountAsync(Builders<Address>.Filter.Empty);
        Assert.Equal<int>(1, counter);

        var address = await addressRepository.GetByCustomerIdAsync("636fe7c0ed55f012b0a17ebc");
        Assert.NotNull(address);

        var patchAddress = new JsonPatchDocument<Address>();
        patchAddress.Replace<string>(a => a.City, "Acapulco");

        await addressRepository.UpdateAsync(address, patchAddress);
        var addressAfterUpdate = await addressRepository.GetByCustomerIdAsync("636fe7c0ed55f012b0a17ebc");
        Assert.Equal("Acapulco", addressAfterUpdate.City);

        await addressRepository.DeleteManyAsync(Builders<Address>.Filter.Empty);
    }

    #endregion
}
