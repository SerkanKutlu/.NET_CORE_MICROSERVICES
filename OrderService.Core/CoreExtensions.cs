using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderService.Common.Exceptions;
using OrderService.Core.ActionFilters;
using OrderService.Core.HttpClient;
using OrderService.Core.HttpClient.Interfaces;
using Polly;


namespace OrderService.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreExtensions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        services.AddSingleton<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptions<HttpClientProperty>>().Value);
        services.AddSingleton<IHttpRequest, HttpRequest>();
        //services.AddSingleton<ICustomerHelper, CustomerHelper>();
        
        services.AddHttpClient("httpClient")
            .AddPolicyHandler(Policy.TimeoutAsync(20, (context, timeSpan, task) =>
            {
                throw new ServerNotRespondingException();

            }).AsAsyncPolicy<HttpResponseMessage>())
            .AddTransientHttpErrorPolicy(policy => policy.RetryAsync(3, (d, r) =>
            {
                
            }));
        
        //ActionFilters
        services.AddSingleton<ProductExistAttribute>();
        services.AddSingleton<CustomerExistAttribute>();
        
        return services;
    }

    
}