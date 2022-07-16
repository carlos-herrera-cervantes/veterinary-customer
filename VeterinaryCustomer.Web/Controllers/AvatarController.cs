using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Web.Attributes;

namespace VeterinaryCustomer.Web.Controllers
{
    [Route("api/v1/customers/avatars")]
    [Produces("application/json")]
    [Consumes("multipart/form-data")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        #region snippet_Properties

        private readonly IAvatarRepository _avatarRepository;

        #endregion

        #region snippet_Constructors

        public AvatarController(IAvatarRepository avatarRepository)
        {
            _avatarRepository = avatarRepository;
        }

        #endregion

        #region snippet_ActionMethods

        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync([FromHeader(Name = "user-id")] string userId)
        {
            var avatar = await _avatarRepository.GetByCustomerIdAsync(userId);

            if (avatar is null) return NotFound();

            return Ok(avatar);
        }

        [HttpPost]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> CreateAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [Required] [FromForm] IFormFile image
        )
        {
            var avatar = new Avatar
            {
                CustomerId = userId,
                Path = $"{userId}/{image.FileName}"
            };

            await _avatarRepository.CreateAsync(avatar);

            return Created("", avatar);
        }

        [HttpPatch("me")]
        [RequestSizeLimit(2_000_000)]
        public async Task<IActionResult> UpdateMeAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [Required] [FromForm] IFormFile image
        )
        {
            var avatar = await _avatarRepository.GetByCustomerIdAsync(userId);

            if (avatar is null) return NotFound();

            avatar.Path = $"{userId}/{image.FileName}";

            await _avatarRepository.UpdateAsync(avatar);

            return Ok(avatar);
        }

        [HttpDelete("me")]
        [Avatar]
        public async Task<IActionResult> DeleteMeAsync([FromHeader(Name = "user-id")] string userId)
        {
            await _avatarRepository.DeleteAsync(userId);
            return NoContent();
        }

        #endregion
    }
}
