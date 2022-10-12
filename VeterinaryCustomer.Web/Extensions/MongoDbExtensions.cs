using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace VeterinaryCustomer.Web.Extensions;

public static class MongoDbExtensions
{
    public static IServiceCollection AddMongoDbClient(this IServiceCollection serivces)
    {
        string uri = Environment.GetEnvironmentVariable("MONGODB_URI");
        var mongoClient = new MongoClient(uri);

        serivces.AddSingleton<IMongoClient>(_ => mongoClient);

        return serivces;
    }
}
