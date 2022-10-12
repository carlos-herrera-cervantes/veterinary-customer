using System.Threading.Tasks;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;

namespace VeterinaryCustomer.Repositories.Repositories;

public interface IAvatarRepository
{
    Task<int> CountAsync(FilterDefinition<Avatar> filterDefinition);

    Task<Avatar> GetByCustomerIdAsync(string customerId);

    Task CreateAsync(Avatar avatar);

    Task UpdateAsync(Avatar avatar);

    Task DeleteAsync(string customerId);
}
