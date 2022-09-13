using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Core.Entity;

namespace UserService.Core;

public static class DataExtensions
{

    public static IServiceCollection AddCoreExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
    
}