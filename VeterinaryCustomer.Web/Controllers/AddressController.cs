using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;

namespace VeterinaryCustomer.Web.Controllers
{
    [Route("api/v1/customers/address")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        #region snippet_Properties

        private readonly IAddressRepository _addressRepository;

        #endregion

        #region snippet_Constructors

        public AddressController(IAddressRepository addressRepository)
            => _addressRepository = addressRepository;

        #endregion

        #region snippet_ActionMethods

        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync([FromHeader(Name = "user-id")] string userId)
        {
            var address = await _addressRepository.GetByCustomerIdAsync(userId);

            if (address is null) return NotFound();

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromBody] Address address
        )
        {
            address.CustomerId = userId;
            await _addressRepository.CreateAsync(address);
            return Created("", address);
        }

        [HttpPatch("me")]
        public async Task<IActionResult> UpdateMeAsync
        (
            [FromHeader(Name = "user-id")] string userId,
            [FromBody] JsonPatchDocument<Address> patchDocument
        )
        {
            var address = await _addressRepository.GetByCustomerIdAsync(userId);

            if (address is null) return NotFound();
            
            await _addressRepository.UpdateAsync(address, patchDocument);

            return Ok(address);
        }

        #endregion
    }
}
