﻿using System.Reflection;
using CustomerService.Common.Middlewares;
using CustomerService.Common.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace CustomerService.Common;

public static class CommonExtensions
{
    public static IServiceCollection AddCommonExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        //This will add all validators at these assembly. (default and at the documentation : scoped)
        services.AddFluentValidation();
        services.AddValidatorsFromAssemblyContaining<AddressValidation>();
        //Auto Mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
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

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();


        return app;
    }

}