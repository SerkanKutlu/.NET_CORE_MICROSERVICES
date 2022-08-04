using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Application.Helpers;
using OrderService.Application.Interfaces;
using OrderService.Application.Validations;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Options;
using OrderService.Application.Exceptions;
using OrderService.Application.Middlewares;
using OrderService.Infrastructure.HttpClient;
using OrderService.Infrastructure.Mongo;
using OrderService.Infrastructure.Repository;
using OrderService.Infrastructure.Services;
using Polly;
namespace OrderService.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection RegisterComponents(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        AddRepositories(services);
        AddSeriLogConfiguration(builder);
        AddValidations(services);
        AddHelpers(services);
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
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
    }
    
    private static void AddSeriLogConfiguration(WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
            .Enrich
            .FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
    }

    private static void AddValidations(IServiceCollection services)
    {
        //This will add all validators at these assembly. (default and at the documentation : scoped)
        services.AddFluentValidation();
        services.AddValidatorsFromAssemblyContaining<AddressValidator>();
    }

    private static void AddHelpers(IServiceCollection services )
    {
        services.AddScoped<IOrderHelper, OrderHelper>();
        services.AddScoped<IHttpRequest, HttpRequest>();
        services.AddScoped<IPublisher, Publisher>();
    }

    private static void AddConfigurations(IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        services.AddScoped<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptionsSnapshot<HttpClientProperty>>().Value);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<IOrderService, Services.OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddHttpClient("httpClient")
        .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
        {
            throw new ServerNotRespondingException();
        
        }).AsAsyncPolicy<HttpResponseMessage>())
        .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
        {
            
        }));
    }
}