using GenericMongo.Bases;
using GenericMongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace GenericMongo;

public static class Bootstrapper
{
    public static IServiceCollection AddGenericMongo<T>(this IServiceCollection services,
        Action<MongoSettings> settings) where T:BaseEntity
    {
        var mongoSettings = new MongoSettings();
        settings.Invoke(mongoSettings);
        AddRepository<T>(services,mongoSettings);
        return services;
    }
    public static IServiceCollection AddGenericMongo<T>(this IServiceCollection services,
        Action<MongoSettings> settings, Action<IMongoCollection<T>> collection) where T:BaseEntity
    {
        var mongoSettings = new MongoSettings();
        settings.Invoke(mongoSettings);
        AddRepository<T>(services,mongoSettings,collection);
        return services;
    }


    private static void AddRepository<T>(this IServiceCollection services, MongoSettings settings)
    where T:BaseEntity
    {
        services.AddSingleton<IMongoService<T>,MongoService<T>>(sp=>new MongoService<T>(settings));
        services.AddScoped<IRepository<T>, RepositoryBase<T>>();
    }
    private static void AddRepository<T>(this IServiceCollection services, MongoSettings settings,Action<IMongoCollection<T>> collection)
        where T:BaseEntity
    {
        services.AddSingleton<IMongoService<T>,MongoService<T>>(sp=>new MongoService<T>(settings,collection));
        services.AddScoped<IRepository<T>, RepositoryBase<T>>();
    }


}