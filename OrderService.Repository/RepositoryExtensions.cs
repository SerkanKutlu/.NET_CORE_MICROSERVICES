using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Repository.Repository;
using OrderService.Repository.Repository.Interfaces;

namespace OrderService.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddSingleton<IProductRepository, ProductRepository>();
        return services;
    }
}