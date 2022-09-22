using CustomerService.Application.Exceptions;
using CustomerService.Application.Helpers;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Middlewares;
using CustomerService.Application.Validations;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.HttpClient;
using CustomerService.Infrastructure.Kafka;
using CustomerService.Infrastructure.Rabbit;
using CustomerService.Infrastructure.Redis;
using CustomerService.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
using GenericMongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

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
        services.AddGenericMongo<Customer>(settings =>
        {
            settings.CollectionName = configuration.GetSection("MongoSettings")["CollectionName"];
            settings.ConnectionString = configuration.GetSection("MongoSettings")["ConnectionString"];
            settings.DatabaseName = configuration.GetSection("MongoSettings")["DatabaseName"];
        }, collection =>
        {
            var options = new CreateIndexOptions {Unique = true};
            var indexModel = new CreateIndexModel<Customer>("{Email:1}",options);
            collection.Indexes.CreateOne(indexModel);
        });
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
        //services.AddSingleton<IPublisher, Publisher>();
        services.AddScoped<IHttpRequest, HttpRequest>();
    }

    private static void AddConfigurations(IServiceCollection services,IConfiguration configuration)
    {
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        //(IOptionsSnapchat)
        services.AddScoped<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptionsSnapshot<HttpClientProperty>>().Value);
        
        //Redis
        services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));
        services.AddSingleton<IRedisSettings>(provider=>provider.GetRequiredService<IOptions<RedisSettings>>().Value);
        
        //Kafka
        services.Configure<KafkaSettings>(configuration.GetSection(nameof(KafkaSettings)));
        services.AddSingleton<IKafkaSettings>(provider=>provider.GetRequiredService<IOptions<KafkaSettings>>().Value);
        
        //Rabbit
        services.Configure<RabbitSettings>(configuration.GetSection(nameof(RabbitSettings)));
        services.AddSingleton<IRabbitSettings>(provider=>provider.GetRequiredService<IOptions<RabbitSettings>>().Value);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, Services.CustomerService>();
        
        services.AddHttpClient("httpClient")
            .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
            {
                throw new ServerNotRespondingException();

            }).AsAsyncPolicy<HttpResponseMessage>())
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
            {
            }));

        //Redis
        services.AddSingleton<IRedisService, RedisService>();
        services.AddSingleton<IRedisPublisher, RedisPublisher>();
        //Kafka
        services.AddSingleton<IKafkaPublisher, KafkaPublisher>();
        //Rabbit
        services.AddSingleton<IRabbitPublisher, RabbitPublisher>();
    }
}