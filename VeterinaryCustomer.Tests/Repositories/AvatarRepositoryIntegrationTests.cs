using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories;

[Collection("AvatarRepositoryIntegration")]
public class AvatarRepositoryIntegrationTests
{
    #region snippet_Properties

    private readonly IMongoClient _mongoClient;

    #endregion

    #region snippet_Constructors

    public AvatarRepositoryIntegrationTests()
    {
        string uri = Environment.GetEnvironmentVariable("MONGODB_URI");
        _mongoClient = new MongoClient(uri);
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 0 documents")]
    public async Task CountAsyncShouldReturnZeroDocuments()
    {
        var avatarRepository = new AvatarRepository(_mongoClient);
        var counter = await avatarRepository.CountAsync(Builders<Avatar>.Filter.Empty);
        Assert.Equal<int>(0, counter);
    }

    [Fact(DisplayName = "Should return null when documents does not exist")]
    public async Task GetByCustomerIdAsyncShouldReturnNull()
    {
        var avatarRepository = new AvatarRepository(_mongoClient);
        var avatar = await avatarRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.Null(avatar);
    }

    [Fact(DisplayName = "Should apply three operations on document")]
    public async Task CreateUpdateAndDeleteAsyncShouldApplyOperationsOnDocument()
    {
        var newAvatar = new Avatar
        {
            CustomerId = "636ff507696d48e655bebca8",
            Path = "profile.png"
        };
        var avatarRepository = new AvatarRepository(_mongoClient);

        await avatarRepository.CreateAsync(newAvatar);

        var avatar = await avatarRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.NotNull(avatar);

        avatar.Path = "new-profile.png";
        await avatarRepository.UpdateAsync(avatar);

        var avatarAfterUpdate = await avatarRepository.GetByCustomerIdAsync("636ff507696d48e655bebca8");
        Assert.Equal("new-profile.png", avatar.Path);

        await avatarRepository.DeleteAsync("636ff507696d48e655bebca8");
    }

    #endregion
}
