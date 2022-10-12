using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Web.Controllers;
using VeterinaryCustomer.Web.Types;
using Xunit;

namespace VeterinaryCustomer.Tests.Controllers;

[Collection("ProfileController")]
public class ProfileControllerTests
{
    #region snippet_Properties

    private readonly Mock<IProfileRepository> _mockProfileRepository;

    #endregion

    #region snippet_Constructors

    public ProfileControllerTests()
        => _mockProfileRepository = new Mock<IProfileRepository>();

    #endregion

    #region snippet_Tests

    [Fact(DisplayName = "Should return 200 status code response when listing profiles")]
    public async Task GetAllAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Profile> { });
        _mockProfileRepository
            .Setup(x => x.CountAsync(It.IsAny<FilterDefinition<Profile>>()))
            .ReturnsAsync(0);

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.GetAllAsync(new Pager { Page = 0, PageSize = 10 });
        var okObjectResult = response as OkObjectResult;
        var body = okObjectResult.Value as Pagination<Profile>;

        _mockProfileRepository.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _mockProfileRepository.Verify(x => x.CountAsync(It.IsAny<FilterDefinition<Profile>>()), Times.Once);

        Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        Assert.Equal(0, body.Previous);
        Assert.Equal(0, body.Next);
        Assert.Equal(0, body.Total);
        Assert.Empty(body.Data);
    }

    [Fact(DisplayName = "Should return 404 status code response when profile does not exist")]
    public async Task GetByIdAsyncShouldReturn404()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.GetByIdAsync("dummy-id");
        var notFoundResult = response as NotFoundResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

        Assert.IsType<NotFoundResult>(response);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact(DisplayName = "Should return 200 status code response when profile exist")]
    public async Task GetByIdAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Profile { });

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.GetByIdAsync("dummy-id");
        var okObjectResult = response as OkObjectResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

        Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact(DisplayName = "Should return 404 status code response when profile does not exist")]
    public async Task GetMeAsyncShouldReturn404()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.GetMeAsync("dummy-id");
        var notFoundResult = response as NotFoundResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

        Assert.IsType<NotFoundResult>(response);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact(DisplayName = "Should return 200 status code response when profile exist")]
    public async Task GetMeAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Profile { });

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.GetMeAsync("dummy-id");
        var okObjectResult = response as OkObjectResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);

        Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    [Fact(DisplayName = "Should return 404 status code response when update profile")]
    public async Task UpdateMeAsyncShouldReturn404()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.UpdateMeAsync("dummy-id", new UpdateProfileDto { });
        var notFoundResult = response as NotFoundResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
        _mockProfileRepository.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Never);

        Assert.IsType<NotFoundResult>(response);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact(DisplayName = "Should return 200 status code response when update profile")]
    public async Task UpdateMeAsyncShouldReturn200()
    {
        _mockProfileRepository
            .Setup(x => x.GetByCustomerIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new Profile { });
        _mockProfileRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Profile>()))
            .Returns(Task.FromResult(true));

        var profileController = new ProfileController(_mockProfileRepository.Object);
        var response = await profileController.UpdateMeAsync("dummy-id", new UpdateProfileDto { });
        var okObjectResult = response as OkObjectResult;

        _mockProfileRepository.Verify(x => x.GetByCustomerIdAsync(It.IsAny<string>()), Times.Once);
        _mockProfileRepository.Verify(x => x.UpdateAsync(It.IsAny<Profile>()), Times.Once);

        Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
    }

    #endregion
}
