using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserService.Core.Interfaces;
using UserService.Infrastructure.Mongo;
using UserService.Infrastructure.Repository;

namespace UserService.Infrastructure;

public static class RepositoryExtensions
{
    public static IServiceCollection AddInfrastructreExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<ITokenHandler, TokenHandler>(sp=>new TokenHandler(configuration));
        return services;
    }
}