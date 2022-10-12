using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using VeterinaryCustomer.Services.Aws;
using Xunit;

namespace VeterinaryCustomer.Tests.Services;

[Collection("S3Service")]
public class S3ServiceTests
{
    #region snippet_Properties

    private readonly Mock<IAmazonS3> _mockS3Client;

    #endregion

    #region snippet_Constructors

    public S3ServiceTests()
    {
        _mockS3Client = new Mock<IAmazonS3>();
    }

    #endregion

    #region snippet_Tests

    [Fact]
    public async Task PutObjectAsyncShouldReturnString()
    {
        _mockS3Client
            .Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PutObjectResponse());

        var s3Service = new S3Service(_mockS3Client.Object);

        var key = "myavatar.png";
        var userId = "dummyid";
        var fileStream = new Mock<Stream>();
        var fullPath = await s3Service.PutObjectAsync(key, userId, fileStream.Object);

        _mockS3Client.Verify
            (
                x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()),
                Times.Once
            );

        var fullPathExpected = "http://localhost:4566/veterinary-user-avatars-dev/dummyid/myavatar.png";

        Assert.Matches(fullPathExpected, fullPath);
    }

    [Fact]
    public async Task DeleteObjectAsyncShouldComplete()
    {
        _mockS3Client
            .Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteObjectResponse());

        var s3Service = new S3Service(_mockS3Client.Object);

        var key = "dummyid/myavatar.png";
        await s3Service.DeleteObjectAsync(key);

        _mockS3Client.Verify
        (
            x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    #endregion
}
