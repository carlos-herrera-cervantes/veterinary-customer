using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories;

[Collection("AddressRepository")]
public class AddressRepositoryTests
{
    #region snippet_Properties

    private readonly Mock<IMongoClient> _mockMongoClient;

    private readonly Mock<IMongoCollection<Address>> _mockMongoCollection;

    private readonly Mock<IMongoDatabase> _mockMongoDatabase;

    private readonly Mock<IAsyncCursor<Address>> _mockMongoCursor;

    #endregion

    #region snippet_Constructors

    public AddressRepositoryTests()
    {
        _mockMongoClient = new Mock<IMongoClient>();
        _mockMongoCollection = new Mock<IMongoCollection<Address>>();
        _mockMongoDatabase = new Mock<IMongoDatabase>();
        _mockMongoCursor = new Mock<IAsyncCursor<Address>>();
    }

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return an integer")]
    public async Task CountAsyncShouldReturnInt()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Address>("addresses", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.CountDocumentsAsync
            (
                It.IsAny<FilterDefinition<Address>>(),
                default,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(1);

        var addressRepository = new AddressRepository(_mockMongoClient.Object);
        var filter = Builders<Address>.Filter.Empty;
        var result = await addressRepository.CountAsync(filter);

        _mockMongoCollection
            .Verify(x => x.CountDocumentsAsync
            (
                It.IsAny<FilterDefinition<Address>>(),
                default,
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        Assert.Equal(1, result);
    }

    [Fact(DisplayName = "Should return null when address does not exist")]
    public async Task GetByCustomerIdAsyncShouldReturnNull()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Address>("addresses", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.FindAsync
            (
                It.IsAny<FilterDefinition<Address>>(),
                It.IsAny<FindOptions<Address, Address>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(_mockMongoCursor.Object);

        var addressRepository = new AddressRepository(_mockMongoClient.Object);
        var result = await addressRepository.GetByCustomerIdAsync("dummy-id");

        _mockMongoCollection
            .Verify(x => x.FindAsync
            (
                It.IsAny<FilterDefinition<Address>>(),
                It.IsAny<FindOptions<Address, Address>>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        Assert.Null(result);
    }

    [Fact(DisplayName = "Should call insert one async method")]
    public async Task CreateAsyncShouldCreateAddress()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Address>("addresses", default))
            .Returns(_mockMongoCollection.Object);
        _mockMongoCollection
            .Setup(x => x.InsertOneAsync
            (
                It.IsAny<Address>(),
                default,
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.FromResult(true));

        var addressRepository = new AddressRepository(_mockMongoClient.Object);
        await addressRepository.CreateAsync(new Address { });

        _mockMongoCollection
            .Verify(x => x.InsertOneAsync
                (
                    It.IsAny<Address>(),
                    default,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    [Fact(DisplayName = "Should call the replace one async method")]
    public async Task UpdateByCustomerIdAsyncShouldUpdateAddress()
    {
        _mockMongoClient
            .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
            .Returns(_mockMongoDatabase.Object);
        _mockMongoDatabase
            .Setup(x => x.GetCollection<Address>("addresses", default))
            .Returns(_mockMongoCollection.Object);

        var mockReplaceOneResult = new Mock<ReplaceOneResult>();

        _mockMongoCollection
            .Setup(x => x.ReplaceOneAsync
            (
                It.IsAny<FilterDefinition<Address>>(),
                It.IsAny<Address>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()
            ))
            .Returns(Task.FromResult(mockReplaceOneResult.Object));

        var addressRepository = new AddressRepository(_mockMongoClient.Object);
        await addressRepository.UpdateAsync(new Address { }, new JsonPatchDocument<Address> { });

        _mockMongoCollection
            .Verify(x => x.ReplaceOneAsync
                (
                    It.IsAny<FilterDefinition<Address>>(),
                    It.IsAny<Address>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
    }

    #endregion
}
