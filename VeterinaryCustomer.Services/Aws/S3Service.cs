using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace VeterinaryCustomer.Services.Aws
{
    public class S3Service : IS3Service
    {
        #region snippet_Properties

        private readonly IAmazonS3 _s3Client;

        #endregion

        #region snippet_Constructors

        public S3Service(IAmazonS3 s3Client) => _s3Client = s3Client;

        #endregion

        #region snippet_ActionMethods

        public async Task<string> PutObjectAsync(string key, string userId, Stream fileStream)
        {
            var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET");
            var endpoint = Environment.GetEnvironmentVariable("S3_ENDPOINT");
            var request = new PutObjectRequest
            {
                InputStream = fileStream,
                BucketName = bucketName,
                Key = $"{userId}/{key}"
            };

            await _s3Client.PutObjectAsync(request);
            return $"{endpoint}/{bucketName}/{userId}/{key}";
        }

        public async Task DeleteObjectAsync(string key)
        {
            var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET");
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        #endregion
    }
}
