using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserService.Data.Mongo;
using UserService.Data.Repository;


namespace UserService.Data;

public static class DataExtensions
{

    public static IServiceCollection AddDataExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<IUserRepository, UserRepository>();
        //Auto Mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
       
        return services;
    }
    
}