using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

public class ProductService:IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    
    public ProductService(IProductRepository orderRepository, ILogger<ProductService> logger,IOrderRepository orderRepository1)
    {
        _productRepository = orderRepository;
        _logger = logger; 
        _orderRepository = orderRepository1;
    }
    public async Task<PagedList<Product>> GetProductsPaged(RequestParameters requestParameters,HttpContext context)
    {
        var products = await _productRepository.GetAll(requestParameters);
        context.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(products.MetaData));
        _logger.LogInformation("Getting all products data's from database");
        return products;
    }
    public async Task<Product> GetById(string id)
    {
        var product = await _productRepository.GetWithId(id);
        _logger.LogInformation($"Getting product data with {id} from database");
        return product;
    }
    
    public async Task<string> CreateProduct(ProductForCreationDto productForCreation, HttpContext context)
    {
        var product = productForCreation.ToProduct();
        await _productRepository.CreateAsync(product);
        context.Response.Headers.Add("location",$"https://{context.Request.Headers["Host"]}/api/Products/{product.Id}");
        _logger.LogInformation($"New product added with id {product.Id}");
        return product.Id;
    }
    
    public async Task<Product> UpdateProduct(ProductForUpdateDto productForUpdate)
    {
        var product = productForUpdate.ToProduct();
        await _productRepository.UpdateAsync(product);
        _logger.LogInformation($"Product with id {product.Id} updated.");
        return product;
    }
    
    public async Task DeleteProduct(string id)
    {
        //Delete product
        var product =  _productRepository.GetWithId(id);
        var deletingTask =  _productRepository.DeleteAsync(id);
        await product; 
        await _orderRepository.UpdateProductRelatedOrders(product.Result);
        await deletingTask;
        //Logging and return
        _logger.LogInformation($"Product with id {id} deleted.");
    }
}