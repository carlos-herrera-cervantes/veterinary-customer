using System;
using System.Globalization;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories;

[Collection("ProfileRepositoryIntegration")]
public class ProfileRepositoryIntegrationTests
{
    #region snippet_Properties

    private readonly IMongoClient _mongoClient;

    #endregion

    #region snippet_Constructors

    public ProfileRepositoryIntegrationTests()
    {
        string uri = Environment.GetEnvironmentVariable("MONGODB_URI");
        _mongoClient = new MongoClient(uri);
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 0 documents")]
    public async Task CountAsyncShouldReturnZeroDocuments()
    {
        var profileRepository = new ProfileRepository(_mongoClient);
        var counter = await profileRepository.CountAsync(Builders<Profile>.Filter.Empty);
        Assert.Equal<int>(0, counter);
    }

    [Fact(DisplayName = "Should return an empty list")]
    public async Task GetAllAsyncShouldReturnEmptyList()
    {
        var profileRepository = new ProfileRepository(_mongoClient);
        var profiles = await profileRepository.GetAllAsync(0, 10);
        Assert.Empty(profiles);
    }

    [Fact(DisplayName = "Should return null when document does not exist")]
    public async Task GetByCustomerIdAsyncShouldReturnNull()
    {
        var profileRepository = new ProfileRepository(_mongoClient);
        var profile = await profileRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.Null(profile);
    }

    [Fact(DisplayName = "Should update a document")]
    public async Task UpdateAsyncShouldUpdateDocument()
    {
        var profileRepository = new ProfileRepository(_mongoClient);
        var newProfile = new Profile
        {
            CustomerId = "636ff507696d48e655bebca8",
            Name = "Andrés",
            LastName = "Obrador",
            PhoneNumber = "12345",
            Email = "user@example.com",
            Birthday = DateTime.ParseExact("1990-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
        };

        await profileRepository.CreateAsync(newProfile);

        var profile = await profileRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.NotNull(profile);

        profile.Name = "Enrique";
        await profileRepository.UpdateAsync(profile);

        var profileAfterUpdate = await profileRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.Equal("Enrique", profileAfterUpdate.Name);

        await profileRepository.DeleteManyAsync(Builders<Profile>.Filter.Empty);
    }

    #endregion
}
