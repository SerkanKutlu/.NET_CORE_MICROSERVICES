using CustomerService.Repository.CustomerRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository.CustomerRepository>();
        return services;
    }
}