using CustomerService.Domain.Entities;
using CustomerService.Domain.ValueObjects;
using MongoDB.Driver;

namespace CustomerService.Infrastructure.Mongo;

public static class  SeedCustomerData
{

    public static void SeedData(IMongoCollection<Customer> customers)
    {
        var customerList = customers.Find(c => true).Limit(1).ToList();
        if (!customerList.Any())
        {
            customers.InsertMany(GetInitialCustomers());
        }
        
    }
    private static IEnumerable<Customer>  GetInitialCustomers()
    {
        return new[]
        {
            new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Address = new Address
                {
                    AddressLine = "cumhuriyet",
                    City = "samsun",
                    CityCode = 55,
                    Country = "tr"
                },
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Email = "kutluserkan1@gmail.com",
                Name = "Serkan"
            }
        };
    }
    
}