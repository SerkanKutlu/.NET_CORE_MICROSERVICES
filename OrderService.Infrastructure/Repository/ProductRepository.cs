using MongoDB.Driver;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;
    public ProductRepository(IMongoService mongoService)
    {
        _products = mongoService.Products;
    }

   
    public async Task CreateAsync(Product newProduct)
    {
        await _products.InsertOneAsync(newProduct);
    }

    
    public async Task UpdateAsync(Product updatedProduct)
    {
        var result = await _products.ReplaceOneAsync(p => p.Id == updatedProduct.Id, updatedProduct);
        if (!result.IsModifiedCountAvailable && result.ModifiedCount == 0)
            throw new NotFoundException<Product>(updatedProduct.Id);
    }
        
        
    public async Task DeleteAsync(string productId)
    { 
        var result = await _products.DeleteOneAsync(p => p.Id == productId);
        if (result.DeletedCount == 0)
            throw new NotFoundException<Product>(productId);
    }
     
    public async Task<PagedList<Product>> GetAll(RequestParameters requestParameters)
    {
        var products = await _products.
            Search(requestParameters.SearchTerm)
            .CustomSort(requestParameters.OrderBy)
            .ToListAsync();
        var pagedProducts =
            PagedList<Product>.ToPagedList(products, requestParameters.PageNumber, requestParameters.PageSize);
        return pagedProducts;
    }

    public async Task<Product> GetWithId(string productId)
    {
        var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        if (product == null)
            throw new NotFoundException<Product>(productId);
        return product;
    }
}