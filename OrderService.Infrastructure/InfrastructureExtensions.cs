using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.HttpClient;
using OrderService.Infrastructure.Mongo;
using OrderService.Infrastructure.Repository;
using Polly;

namespace OrderService.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
        services.AddSingleton<IMongoService, MongoService>();
        
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        services.AddSingleton<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptions<HttpClientProperty>>().Value);
        services.AddSingleton<IHttpRequest, HttpRequest>();
        
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        services.AddHttpClient("httpClient")
            .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
            {
                throw new ServerNotRespondingException();

            }).AsAsyncPolicy<HttpResponseMessage>())
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
            {
                
            }));
        return services;
    }

    
}