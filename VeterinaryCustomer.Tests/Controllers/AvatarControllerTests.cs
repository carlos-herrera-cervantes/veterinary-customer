using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Web.Controllers;
using Xunit;

namespace VeterinaryCustomer.Tests.Controllers
{
    [Collection("AvatarController")]
    public class AvatarControllerTests
    {
        #region snippet_Properties

        private readonly Mock<IAvatarRepository> _mockAvatarRepository;

        #endregion

        #region snippet_Constructors

        public AvatarControllerTests()
        {
            _mockAvatarRepository = new Mock<IAvatarRepository>();
        }

        #endregion

        #region snippet_Tests

        [Fact(DisplayName = "Should return 404 status code response when avatar does not exist")]
        public async Task GetMeAsyncShouldReturn404()
        {
            _mockAvatarRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.GetMeAsync("dummy-id");
            var notFoundResult = response as NotFoundResult;

            _mockAvatarRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 200 status code response when avatar exist")]
        public async Task GetMeAsyncShouldReturn200()
        {
            _mockAvatarRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(new Avatar{});

            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.GetMeAsync("dummy-id");
            var okObjectResult = response as OkObjectResult;

            _mockAvatarRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 201 status code response when avatar is created")]
        public async Task CreateAsyncShouldReturn201()
        {
            _mockAvatarRepository
                .Setup(x => x.CreateAsync(It.IsAny<Avatar>())).Returns(Task.FromResult(true));
            
            var mockFile = new Mock<IFormFile>();

            mockFile.Setup(x => x.FileName).Returns("dummy-avatar.png");

            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.CreateAsync("dummy-id", mockFile.Object);
            var createdResult = response as CreatedResult;

            _mockAvatarRepository.Verify(x => x.CreateAsync(It.IsAny<Avatar>()), Times.Once);

            Assert.IsType<CreatedResult>(response);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 404 status code response when avatar does not exist")]
        public async Task UpdateMeAsyncShouldReturn404()
        {
            _mockAvatarRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var mockFile = new Mock<IFormFile>();
            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.UpdateMeAsync("dummy-id", mockFile.Object);
            var notFoundResult = response as NotFoundResult;

            _mockAvatarRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
            _mockAvatarRepository.Verify(x => x.UpdateAsync(It.IsAny<Avatar>()), Times.Never);

            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 200 status code response when update the avatar")]
        public async Task UpdateMeAsyncShouldReturn200()
        {
            _mockAvatarRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(new Avatar{});
            _mockAvatarRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Avatar>())).Returns(Task.FromResult(true));

            var mockFile = new Mock<IFormFile>();

            mockFile.Setup(x => x.FileName).Returns("dummy-avatar.png");

            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.UpdateMeAsync("dummy-id", mockFile.Object);
            var okObjectResult = response as OkObjectResult;

            _mockAvatarRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
            _mockAvatarRepository.Verify(x => x.UpdateAsync(It.IsAny<Avatar>()), Times.Once);

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 204 status code response when avatar is deleted")]
        public async Task DeleteMeAsyncShouldReturn204()
        {
            _mockAvatarRepository
                .Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            
            var avatarController = new AvatarController(_mockAvatarRepository.Object);
            var response = await avatarController.DeleteMeAsync("dummy-id");
            var noContentResult = response as NoContentResult;

            _mockAvatarRepository.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);

            Assert.IsType<NoContentResult>(response);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        #endregion
    }
}
