using GenericMongo.Bases;
using GenericMongo.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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


    private static void AddRepository<T>(this IServiceCollection services, MongoSettings settings)
    where T:BaseEntity
    {
        services.AddSingleton<IMongoService<T>,MongoService<T>>(sp=>new MongoService<T>(settings));
        services.AddScoped<IRepository<T>, RepositoryBase<T>>();
    }


}