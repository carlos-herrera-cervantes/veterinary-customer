using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Web.Types;
using VeterinaryCustomer.Web.Config;

namespace VeterinaryCustomer.Web.Controllers;

[Route($"{ApiConfig.BasePath}/v1/profiles")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
public class ProfileController : ControllerBase
{
    #region snippet_Properties

    private readonly IProfileRepository _profileRepository;

    #endregion

    #region snippet_Constructors

    public ProfileController(IProfileRepository profileRepository)
        => _profileRepository = profileRepository;

    #endregion

    #region snippet_ActionMethods

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] Pager pager)
    {
        var (page, pageSize) = pager;
        var profiles = await _profileRepository.GetAllAsync(page, pageSize);
        var totalDocs = await _profileRepository.CountAsync(Builders<Profile>.Filter.Empty);
        var pagination = new Pagination<Profile>
        {
            Total = totalDocs,
            Data = profiles
        };

        return Ok(pagination.CalculatePagination(pager));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var profile = await _profileRepository.GetByCustomerIdAsync(id);

        if (profile is null) return NotFound();

        return Ok(profile);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync([FromHeader(Name = "user-id")] string userId)
    {
        var profile = await _profileRepository.GetByCustomerIdAsync(userId);

        if (profile is null) return NotFound();

        return Ok(profile);
    }

    [HttpPatch("me")]
    public async Task<IActionResult> UpdateMeAsync
    (
        [FromHeader(Name = "user-id")] string userId,
        [FromBody] UpdateProfileDto changes
    )
    {
        var profile = await _profileRepository.GetByCustomerIdAsync(userId);

        if (profile is null) return NotFound();

        profile.Name = changes.Name ?? profile.Name;
        profile.LastName = changes.LastName ?? profile.LastName;
        profile.Gender = changes.Gender ?? profile.Gender;
        profile.Birthday = changes.Birthday ?? profile.Birthday;

        await _profileRepository.UpdateAsync(profile);

        return Ok(profile);
    }

    #endregion
}
