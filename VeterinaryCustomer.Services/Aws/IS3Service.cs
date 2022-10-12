using System.IO;
using System.Threading.Tasks;

namespace VeterinaryCustomer.Services.Aws;

public interface IS3Service
{
    Task<string> PutObjectAsync(string key, string userId, Stream fileStream);

    Task DeleteObjectAsync(string key);
}
