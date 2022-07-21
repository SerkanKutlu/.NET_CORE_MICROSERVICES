using CustomerService.Data.Mongo;
using CustomerService.Data.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CustomerService.Data;

public static class DataExtensions
{

    public static IServiceCollection AddDataServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        services.AddSingleton<IMongoService, MongoService>();
        return services;
    }
    
}