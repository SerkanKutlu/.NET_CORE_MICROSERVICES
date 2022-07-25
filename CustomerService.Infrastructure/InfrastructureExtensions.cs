using CustomerService.Application.Exceptions;
using CustomerService.Application.Interfaces;
using CustomerService.Infrastructure.HttpClient;
using CustomerService.Infrastructure.Mongo;
using CustomerService.Infrastructure.Repository;
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
        services.AddSingleton<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptions<HttpClientProperty>>().Value);
        
        services.AddSingleton<IHttpRequest, HttpRequest>();
        
        services.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
        services.AddSingleton<IMongoSettings>(provider=>provider.GetRequiredService<IOptions<MongoSettings>>().Value);
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
        return services;
    }
}