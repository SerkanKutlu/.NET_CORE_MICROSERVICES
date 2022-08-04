using CustomerService.Application.Exceptions;
using CustomerService.Application.Helpers;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Middlewares;
using CustomerService.Application.Validations;
using CustomerService.Infrastructure.HttpClient;
using CustomerService.Infrastructure.Mongo;
using CustomerService.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Serilog;
using Serilog.Events;

namespace CustomerService.Infrastructure;

public static class Bootstrapper
{
    public static IServiceCollection RegisterComponents(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        AddRepositories(services,configuration);
        AddSeriLogConfiguration(builder);
        AddValidations(services);
        AddHelpers(services,configuration);
        AddConfigurations(services,configuration);
        AddServices(services);
        return services;
    }

    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    private static void AddRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
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
        services.AddValidatorsFromAssemblyContaining<AddressValidation>();
    }

    private static void AddHelpers(IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<ICustomerHelper, CustomerHelper>();
        services.AddSingleton<IPublisher, Publisher>();
        services.AddScoped<IHttpRequest, HttpRequest>();
    }

    private static void AddConfigurations(IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        
        //(IOptionsSnapchat)
        services.AddScoped<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptionsSnapshot<HttpClientProperty>>().Value);
        
        
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<ICustomerService, Services.CustomerService>();
        
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