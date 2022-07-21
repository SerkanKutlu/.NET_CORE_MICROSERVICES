using CustomerService.Common.Exceptions;
using CustomerService.Core.Helpers;
using CustomerService.Core.HttpClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;

namespace CustomerService.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<HttpClientProperty>(configuration.GetSection(nameof(HttpClientProperty)));
        services.AddSingleton<IHttpClientProperty>(provider=>provider.GetRequiredService<IOptions<HttpClientProperty>>().Value);
        services.AddSingleton<IHttpRequest, HttpRequest>();
        services.AddSingleton<ICustomerHelper, CustomerHelper>();
        
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