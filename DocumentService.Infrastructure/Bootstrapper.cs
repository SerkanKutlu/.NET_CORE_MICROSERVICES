using Core.Entity;
using Core.Helpers;
using Core.Interfaces;
using Core.Middlewares;
using DocumentService.Infrastructure.Repository;
using GenericMongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection RegisterComponents(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services,configuration);
        AddServices(services);
        AddHelpers(services);
        return services;
    }
    
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    private static void AddRepositories(IServiceCollection services,IConfiguration configuration)
    {
        services.AddGenericMongo<DocumentEntity>(settings =>
        {
            settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
            settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
            settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
        });
        services.AddScoped<IDocumentRepository, DocumentRepository>();
    }
    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IDocumentService, Services.DocumentService>();
    }

    private static void AddHelpers(IServiceCollection services)
    {
        services.AddScoped<IAuthHelper, AuthHelper>();
        services.AddScoped<IFileHelper, FileHelper>();
    }
}