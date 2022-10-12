using System;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;

namespace VeterinaryCustomer.Web.Extensions;

public static class S3Extensions
{
    public static IServiceCollection AddS3Client(this IServiceCollection services)
    {
        var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
        var credentials = new BasicAWSCredentials(accessKey, secretKey);

        var s3Client = new AmazonS3Client(credentials, new AmazonS3Config
        {
            ServiceURL = Environment.GetEnvironmentVariable("S3_ENDPOINT"),
            UseHttp = true,
            ForcePathStyle = true,
            AuthenticationRegion = Environment.GetEnvironmentVariable("S3_REGION")
        });

        services.AddSingleton<IAmazonS3>(_ => s3Client);

        return services;
    }
}
