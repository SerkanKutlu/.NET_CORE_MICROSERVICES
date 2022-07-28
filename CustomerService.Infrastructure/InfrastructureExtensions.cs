using CustomerService.Application.Exceptions;
using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.HttpClient;
using CustomerService.Infrastructure.Mongo;
using CustomerService.Infrastructure.Publishers;
using CustomerService.Infrastructure.Repository;
using CustomerService.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;

namespace CustomerService.Infrastructure;

public static class ApplicationExtensions
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        
        //These are fine with scoped.
        services.AddScoped<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptionsSnapshot<HttpClientProperty>>().Value);
        services.AddScoped<IHttpRequest, HttpRequest>();
        
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        //Has to be singleton
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        
        //Definitely correct
        services.AddSingleton<IMongoService, MongoService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        services.AddHttpClient("httpClient")
            .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
            {
                throw new ServerNotRespondingException();

            }).AsAsyncPolicy<HttpResponseMessage>())
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
            {
            }));

        services.AddScoped<ICustomerRequestService, CustomerRequestService>();
        services.AddScoped<IPublisher, LogPublisher>();
        
        //Mass Transit
        services.AddMassTransit(x =>
        {
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            }));
        });
        return services;
    }
}