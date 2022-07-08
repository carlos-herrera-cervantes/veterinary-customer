using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using Xunit;

namespace VeterinaryCustomer.Tests.Repositories
{
    [Collection("ProfileRepository")]
    public class ProfileRepositoryTests
    {
        #region snippet_Properties

        private readonly Mock<IMongoClient> _mockMongoClient;

        private readonly Mock<IMongoCollection<Profile>> _mockMongoCollection;

        private readonly Mock<IMongoDatabase> _mockMongoDatabase;

        private readonly Mock<IAsyncCursor<Profile>> _mockMongoCursor;

        #endregion

        #region snippet_Constructors

        public ProfileRepositoryTests()
        {
            _mockMongoClient = new Mock<IMongoClient>();
            _mockMongoCollection = new Mock<IMongoCollection<Profile>>();
            _mockMongoDatabase = new Mock<IMongoDatabase>();
            _mockMongoCursor = new Mock<IAsyncCursor<Profile>>();
        }

        #endregion

        #region snippet_Tests

        [Fact(DisplayName = "Should return a integer")]
        public async Task CountAsyncShouldReturnInt()
        {
            _mockMongoClient
                .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mockMongoDatabase.Object);
            _mockMongoDatabase
                .Setup(x => x.GetCollection<Profile>("profiles", default))
                .Returns(_mockMongoCollection.Object);
            _mockMongoCollection
                .Setup(x => x.CountDocumentsAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        default,
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(1);

            var profileRepository = new ProfileRepository(_mockMongoClient.Object);
            var filter = Builders<Profile>.Filter.Empty;
            var result = await profileRepository.CountAsync(filter);

            _mockMongoCollection
                .Verify(x => x.CountDocumentsAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        default,
                        It.IsAny<CancellationToken>()
                    ),
                    Times.Once
                );

            Assert.Equal(1, result);
        }

        [Fact(DisplayName = "Should return null when customer does not exist")]
        public async Task GetByCustomerIdAsyncShouldReturnNull()
        {
            _mockMongoClient
                .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mockMongoDatabase.Object);
            _mockMongoDatabase
                .Setup(x => x.GetCollection<Profile>("profiles", default))
                .Returns(_mockMongoCollection.Object);
            _mockMongoCollection
                .Setup(x => x.FindAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        It.IsAny<FindOptions<Profile, Profile>>(),
                        It.IsAny<CancellationToken>()
                    ))
                .ReturnsAsync(_mockMongoCursor.Object);

            var profileRepository = new ProfileRepository(_mockMongoClient.Object);
            var result = await profileRepository.GetByCustomerIdAsync("dummy-id");

            _mockMongoCollection
                .Verify(x => x.FindAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        It.IsAny<FindOptions<Profile, Profile>>(),
                        It.IsAny<CancellationToken>()
                    ),
                    Times.Once
                );

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should call the replace one async method")]
        public async Task UpdateAsyncShouldUpdateProfile()
        {
            _mockMongoClient
                .Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mockMongoDatabase.Object);
            _mockMongoDatabase
                .Setup(x => x.GetCollection<Profile>("profiles", default))
                .Returns(_mockMongoCollection.Object);

            var mockReplaceOneResult = new Mock<ReplaceOneResult>();

            _mockMongoCollection
                .Setup(x => x.ReplaceOneAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        It.IsAny<Profile>(), It.IsAny<ReplaceOptions>(),
                        It.IsAny<CancellationToken>()
                    ))
                .Returns(Task.FromResult(mockReplaceOneResult.Object));

            var profileRepository = new ProfileRepository(_mockMongoClient.Object);
            await profileRepository.UpdateAsync(new Profile{});

            _mockMongoCollection
                .Verify(x => x.ReplaceOneAsync
                    (
                        It.IsAny<FilterDefinition<Profile>>(),
                        It.IsAny<Profile>(),
                        It.IsAny<ReplaceOptions>(),
                        It.IsAny<CancellationToken>()
                    ),
                    Times.Once
                );
        }

        #endregion
    }
}
