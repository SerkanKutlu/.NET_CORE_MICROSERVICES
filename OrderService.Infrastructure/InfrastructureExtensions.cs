using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderService.Application.Consumers;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.HttpClient;
using OrderService.Infrastructure.Mongo;
using OrderService.Infrastructure.Repository;
using OrderService.Infrastructure.Services;
using Polly;

namespace OrderService.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        //has to be
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        //definitely correct
        services.AddSingleton<IMongoService, MongoService>();
        
        //Scoped
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        services.AddScoped<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptionsSnapshot<HttpClientProperty>>().Value);
        services.AddScoped<IHttpRequest, HttpRequest>();
        
        //definitely correct
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        //Unknown
        services.AddScoped<IPublisher, Publisher>();
        services.AddScoped<IOrderRequestService, OrderRequestService>();
        services.AddHttpClient("httpClient")
            .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
            {
                throw new ServerNotRespondingException();

            }).AsAsyncPolicy<HttpResponseMessage>())
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
            {
                
            }));
        //Mass Transit
        services.AddMassTransit(x =>
        {
            x.AddConsumer<DeleteOrdersConsumer>().Endpoint(c => c.Name = "customer.delete");
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }

    
}