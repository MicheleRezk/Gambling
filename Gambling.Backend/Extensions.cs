using Gambling.Backend.Common;
using Gambling.Backend.Data;
using Gambling.Backend.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Gambling.Backend;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        if (BsonSerializer.LookupSerializer<GuidSerializer>() == null)
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        if (BsonSerializer.LookupSerializer<DateTimeOffsetSerializer>() == null)
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var host = configuration["MongoHost"] ?? "localhost";
            var port = configuration["MongoPort"] ?? "27017";
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = new MongoDbSettings { Host = host, Port = port };
            Console.WriteLine($"DB ConnectionString: {mongoDbSettings.ConnectionString}");
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName)
        where T : IEntity
    {
        services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return new MongoRepository<T>(database, collectionName);
        });

        return services;
    }
}