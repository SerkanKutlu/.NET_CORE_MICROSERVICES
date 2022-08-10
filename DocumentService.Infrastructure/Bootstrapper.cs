using Core.Interfaces;
using Core.Middlewares;
using DocumentService.Infrastructure.Mongo;
using DocumentService.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocumentService.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection RegisterComponents(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddConfigurations(services,configuration);
        AddServices(services);
        return services;
    }
    
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IDocumentRepository, DocumentRepository>();
    }
    private static void AddConfigurations(IServiceCollection services,IConfiguration configuration)
    {
      services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
      services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
    }
    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<IDocumentService, Services.DocumentService>();
    }
}