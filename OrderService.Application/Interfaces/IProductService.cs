using Microsoft.AspNetCore.Http;
using OrderService.Application.DTO;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces;

public interface IProductService
{
    Task<PagedList<Product>> GetProductsPaged(RequestParameters requestParameters, HttpContext context);
    Task<Product> GetById(string id);
    Task<string> CreateProduct(ProductForCreationDto productForCreation, HttpContext context);
    Task<Product> UpdateProduct(ProductForUpdateDto productForUpdate);
    Task DeleteProduct(string id);
}