using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Services.Aws;
using VeterinaryCustomer.Web.Attributes;

namespace VeterinaryCustomer.Web.Controllers;

[Route("api/v1/customers/avatars")]
[Produces("application/json")]
[Consumes("multipart/form-data")]
[ApiController]
public class AvatarController : ControllerBase
{
    #region snippet_Properties

    private readonly IAvatarRepository _avatarRepository;

    private readonly IS3Service _s3Service;

    #endregion

    #region snippet_Constructors

    public AvatarController(IAvatarRepository avatarRepository, IS3Service s3Service)
    {
        _avatarRepository = avatarRepository;
        _s3Service = s3Service;
    }

    #endregion

    #region snippet_ActionMethods

    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync([FromHeader(Name = "user-id")] string userId)
    {
        var avatar = await _avatarRepository.GetByCustomerIdAsync(userId);

        if (avatar is null) return NotFound();

        var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET");
        var s3Endpoint = Environment.GetEnvironmentVariable("S3_ENDPOINT");

        avatar.Path = $"{s3Endpoint}/{bucketName}/{avatar.Path}";

        return Ok(avatar);
    }

    [HttpPost]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> CreateAsync
    (
        [FromHeader(Name = "user-id")] string userId,
        [Required][FromForm] IFormFile image
    )
    {
        var imageStream = image.OpenReadStream();
        var fullPath = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

        var avatar = new Avatar
        {
            CustomerId = userId,
            Path = $"{userId}/{image.FileName}"
        };
        await _avatarRepository.CreateAsync(avatar);
        avatar.Path = fullPath;

        return Created("", avatar);
    }

    [HttpPatch("me")]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> UpdateMeAsync
    (
        [FromHeader(Name = "user-id")] string userId,
        [Required][FromForm] IFormFile image
    )
    {
        var avatar = await _avatarRepository.GetByCustomerIdAsync(userId);

        if (avatar is null) return NotFound();

        var imageStream = image.OpenReadStream();
        var fullPath = await _s3Service.PutObjectAsync(image.FileName, userId, imageStream);

        avatar.Path = $"{userId}/{image.FileName}";
        await _avatarRepository.UpdateAsync(avatar);
        avatar.Path = fullPath;

        return Ok(avatar);
    }

    [HttpDelete("me")]
    [Avatar]
    public async Task<IActionResult> DeleteMeAsync([FromHeader(Name = "user-id")] string userId)
    {
        var avatar = await _avatarRepository.GetByCustomerIdAsync(userId);

        if (avatar is null) return NotFound();

        IEnumerable<Task> tasks = new List<Task>
        {
            _s3Service.DeleteObjectAsync(avatar.Path),
            _avatarRepository.DeleteAsync(userId)
        };
        await Task.WhenAll(tasks);

        return NoContent();
    }

    #endregion
}
