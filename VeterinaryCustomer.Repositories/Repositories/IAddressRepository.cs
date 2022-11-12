using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;

namespace VeterinaryCustomer.Repositories.Repositories;

public interface IAddressRepository
{
    Task<int> CountAsync(FilterDefinition<Address> filterDefinition);

    Task<Address> GetByCustomerIdAsync(string customerId);

    Task CreateAsync(Address address);

    Task UpdateAsync(Address address, JsonPatchDocument<Address> patchDocument);

    Task DeleteManyAsync(FilterDefinition<Address> filter);
}
