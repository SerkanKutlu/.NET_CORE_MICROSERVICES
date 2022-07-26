using MongoDB.Driver;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Mongo;

public static class SeedProductData
{

    public static void SeedData(IMongoCollection<Product> products)
    {
        var productList = products.Find(c => true).Limit(1).ToList();
        if (!productList.Any())
        {
            products.InsertMany(GetInitialProducts());
        }
        
    }
    private static IEnumerable<Product>  GetInitialProducts()
    {
        return new[]
        {
            new Product
            {
                Id = "32d91541a2a411f44df899cf",
                ImageUrl = "xx.com",
                Name = "Phone",
                Price = 5000
                
            }
        };
    }
    
}