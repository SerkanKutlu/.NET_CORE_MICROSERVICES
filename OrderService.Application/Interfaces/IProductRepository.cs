using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IProductRepository
{
    Task CreateAsync(Product newProduct);
    Task UpdateAsync(Product updatedProduct);
    Task DeleteAsync(string productId);
    Task<PagedList<Product>> GetAll(RequestParameters requestParameters);
    Task<Product> GetWithId(string productId);
    
}