using System.Reflection;
using CustomerService.Common.Middlewares;
using CustomerService.Common.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Common;

public static class CommonExtensions
{
    public static IServiceCollection AddCommonExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        //This will add all validators at these assembly. (default and at the documentation : scoped)
        services.AddValidatorsFromAssemblyContaining<AddressValidation>();
        //Auto Mapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }


    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();


        return app;
    }

}