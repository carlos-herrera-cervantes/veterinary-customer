using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories;

[Collection("AvatarRepository")]
public class AvatarRepositoryTests
{
    #region snippet_Properties

    private readonly Mock<IMongoClient> _mockMongoClient;

    private readonly Mock<IMongoCollection<Avatar>> _mockMongoCollection;

    private readonly Mock<IMongoDatabase> _mockMongoDatabase;

    private readonly Mock<IAsyncCursor<Avatar>> _mockMongoCursor;

    #endregion

    #region snippet_Constructors

    public AvatarRepositoryTests()
    {
        _mockMongoClient = new Mock<IMongoClient>();
        _mockMongoCollection = new Mock<IMongoCollection<Avatar>>();
        _mockMongoDatabase = new Mock<IMongoDatabase>();
        _mockMongoCursor = new Mock<IAsyncCursor<Avatar>>();
    }

    #endregion

    #region snippet_ActionMethods

    [Fact(DisplayName = "Should return an integer")]
    public async Task CountAsyncShouldReturnInt()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Avatar>("avatars", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.CountDocumentsAsync
            (
                It.IsAny<FilterDefinition<Avatar>>(),
                default,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(1);

        var avatarRepository = new AvatarRepository(_mockMongoClient.Object);
        var filter = Builders<Avatar>.Filter.Empty;
        var result = await avatarRepository.CountAsync(filter);

        _mockMongoCollection
            .Verify(x => x.CountDocumentsAsync
                (
                    It.IsAny<FilterDefinition<Avatar>>(),
                    default,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

        Assert.Equal(1, result);
    }

    [Fact(DisplayName = "Should return null when avatar does not exist")]
    public async Task GetByCustomerIdAsyncShouldReturnNull()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Avatar>("avatars", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.FindAsync
            (
                It.IsAny<FilterDefinition<Avatar>>(),
                It.IsAny<FindOptions<Avatar, Avatar>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(_mockMongoCursor.Object);

        var avatarRepository = new AvatarRepository(_mockMongoClient.Object);
        var result = await avatarRepository.GetByCustomerIdAsync("dummy-id");

        _mockMongoCollection
            .Verify(x => x.FindAsync
                (
                    It.IsAny<FilterDefinition<Avatar>>(),
                    It.IsAny<FindOptions<Avatar, Avatar>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

        Assert.Null(result);
    }

    [Fact(DisplayName = "Should call insert one async method")]
    public async Task CreateAsyncShouldCreateAvatar()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Avatar>("avatars", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.InsertOneAsync
            (
                It.IsAny<Avatar>(),
                default,
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.FromResult(true));

        var avatarRepository = new AvatarRepository(_mockMongoClient.Object);
        await avatarRepository.CreateAsync(new Avatar { });

        _mockMongoCollection
            .Verify(x => x.InsertOneAsync
                (
                    It.IsAny<Avatar>(),
                    default,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact(DisplayName = "Should call replace one async method")]
    public async Task UpdateAsyncShouldUpdateAvatar()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Avatar>("avatars", default))
            .Returns(_mockMongoCollection.Object);

        var mockReplaceOneResult = new Mock<ReplaceOneResult>();

        _mockMongoCollection
            .Setup(x => x.ReplaceOneAsync
            (
                It.IsAny<FilterDefinition<Avatar>>(),
                It.IsAny<Avatar>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.FromResult(mockReplaceOneResult.Object));

        var avatarRepository = new AvatarRepository(_mockMongoClient.Object);
        await avatarRepository.UpdateAsync(new Avatar { });

        _mockMongoCollection
            .Verify(x => x.ReplaceOneAsync
                (
                    It.IsAny<FilterDefinition<Avatar>>(),
                    It.IsAny<Avatar>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact(DisplayName = "Should call delete one async method")]
    public async Task DeleteAsyncShouldDeleteAvatar()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Avatar>("avatars", default))
            .Returns(_mockMongoCollection.Object);

        var mockDeleteResult = new Mock<DeleteResult>();

        _mockMongoCollection
            .Setup(x => x.DeleteOneAsync
            (
                It.IsAny<FilterDefinition<Avatar>>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.FromResult(mockDeleteResult.Object));

        var avatarRepository = new AvatarRepository(_mockMongoClient.Object);
        await avatarRepository.DeleteAsync("dummy-id");

        _mockMongoCollection
            .Verify(x => x.DeleteOneAsync
                (
                    It.IsAny<FilterDefinition<Avatar>>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    #endregion
}
