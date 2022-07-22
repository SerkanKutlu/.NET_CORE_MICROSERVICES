using OrderService.Common.Models;
using OrderService.Entity.Models;

namespace OrderService.Repository.Repository.Interfaces;

public interface IProductRepository
{
    Task CreateAsync(Product newProduct);
    Task UpdateAsync(Product updatedProduct);
    Task DeleteAsync(string productId);
    Task<PagedList<Product>> GetAll(RequestParameters requestParameters);
    Task<Product> GetWithId(string productId);
    
}