using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;

namespace VeterinaryCustomer.Repositories.Repositories
{
    public interface IProfileRepository
    {
        Task<int> CountAsync(FilterDefinition<Profile> filterDefinition);

        Task<IEnumerable<Profile>> GetAllAsync(int page, int pageSize);

        Task<Profile> GetByCustomerIdAsync(string customerId);

        Task UpdateAsync(Profile profile);
    }
}
