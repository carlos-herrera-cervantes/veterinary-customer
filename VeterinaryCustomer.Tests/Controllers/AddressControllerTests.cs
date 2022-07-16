using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Web.Controllers;
using Xunit;

namespace VeterinaryCustomer.Tests.Controllers
{
    [Collection("AddressController")]
    public class AddressControllerTests
    {
        #region snippet_Properties

        private readonly Mock<IAddressRepository> _mockAddressRepository;

        #endregion

        #region snippet_Constructors

        public AddressControllerTests()
        {
            _mockAddressRepository = new Mock<IAddressRepository>();
        }

        #endregion

        #region snippet_Tests

        [Fact(DisplayName = "Should return 404 status code response when address does not exist")]
        public async Task GetMeAsyncShouldReturn404()
        {
            _mockAddressRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var addressController = new AddressController(_mockAddressRepository.Object);
            var response = await addressController.GetMeAsync("dummy-id");
            var notFoundResult = response as NotFoundResult;

            _mockAddressRepository
                .Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 200 status code response when user exist")]
        public async Task GetMeAsyncShouldReturn200()
        {
            _mockAddressRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(new Address{});

            var addressController = new AddressController(_mockAddressRepository.Object);
            var response = await addressController.GetMeAsync("dummy-id");
            var okObjectResult = response as OkObjectResult;

            _mockAddressRepository
                .Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 201 status code response when address is created")]
        public async Task CreateAsyncShouldReturn201()
        {
            _mockAddressRepository
                .Setup(x => x.CreateAsync(It.IsAny<Address>())).Returns(Task.FromResult(true));

            var addressController = new AddressController(_mockAddressRepository.Object);
            var response = await addressController.CreateAsync("dummy-id", new Address{});
            var createdResult = response as CreatedResult;

            _mockAddressRepository
                .Verify(x => x.CreateAsync(It.IsAny<Address>()), Times.Once);

            Assert.IsType<CreatedResult>(response);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 404 status code response when address does not exist")]
        public async Task UpdateMeAsyncShouldReturn404()
        {
            _mockAddressRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var addressController = new AddressController(_mockAddressRepository.Object);
            var response =  await addressController.UpdateMeAsync("dummy-id", new JsonPatchDocument<Address>());
            var notFoundResult = response as NotFoundResult;

            _mockAddressRepository
                .Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
            _mockAddressRepository
                .Verify(x => x.UpdateAsync(It.IsAny<Address>(), It.IsAny<JsonPatchDocument<Address>>()), Times.Never);

            Assert.IsType<NotFoundResult>(response);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact(DisplayName = "Should return 200 status code response when update the address")]
        public async Task UpdateMeAsyncShouldReturn200()
        {
            _mockAddressRepository
                .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(new Address{});
            _mockAddressRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Address>(), It.IsAny<JsonPatchDocument<Address>>()))
                .Returns(Task.FromResult(true));

            var addressController = new AddressController(_mockAddressRepository.Object);
            var response =  await addressController.UpdateMeAsync("dummy-id", new JsonPatchDocument<Address>());
            var okObjectResult = response as OkObjectResult;

            _mockAddressRepository
                .Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
            _mockAddressRepository
                .Verify(x => x.UpdateAsync(It.IsAny<Address>(), It.IsAny<JsonPatchDocument<Address>>()), Times.Once);

            Assert.IsType<OkObjectResult>(response);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        #endregion
    }
}
