using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VeterinaryCustomer.Repositories.Repositories;
using VeterinaryCustomer.Services.Aws;
using VeterinaryCustomer.Web.Extensions;

namespace VeterinaryCustomer.Web;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson();
        services.AddSingleton<IProfileRepository, ProfileRepository>();
        services.AddSingleton<IAddressRepository, AddressRepository>();
        services.AddSingleton<IAvatarRepository, AvatarRepository>();
        services.AddMongoDbClient();
        services.AddS3Client();
        services.AddSingleton<IS3Service, S3Service>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
