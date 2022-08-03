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

namespace OrderService.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        //This will add all validators at these assembly. (default and at the documentation : scoped)
        services.AddFluentValidation();
        services.AddValidatorsFromAssemblyContaining<AddressValidator>();
        //Has to be scoped
        services.AddScoped<IOrderHelper, OrderHelper>();
        return services;
    }
    public static void AddSeriLogConfiguration(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
            .Enrich
            .FromLogContext()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);
        
        
    }
}