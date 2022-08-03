using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;
using OrderService.Domain.Entities;

namespace OrderServiceClean.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        public ProductController(IProductRepository orderRepository, ILogger<ProductController> logger, IMapper mapper, IOrderRepository orderRepository1)
        {
            _productRepository = orderRepository;
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository1;
        }
        #region Get Requests

        /// <summary>
        /// List all products
        /// </summary>
        /// <param name="requestParameters">Paging params if needed</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
        {
            var products = await _productRepository.GetAll(requestParameters);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(products.MetaData));
            _logger.LogInformation("Getting all products data's from database");
            return Ok(products);
        }
        
        /// <summary>
        /// Fetch a product with giving id
        /// </summary>
        ///<param name="id">Id of specific product</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Product Id</response>
        /// <response code="500">Server Error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productRepository.GetWithId(id);
            _logger.LogInformation($"Getting product data with {id} from database");
            return Ok(product);
        }
        #endregion
        
        #region Post Requests

        /// <summary>
        /// Creating new product by using body.
        /// </summary>
        /// <param name="productForCreation">All areas are required</param>
        /// <response code="201">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto productForCreation)
        {
            var product = _mapper.Map<Product>(productForCreation);
            await _productRepository.CreateAsync(product);
            _logger.LogInformation($"New product added with id {product.Id}");
            return CreatedAtAction("GetById", new {id = product.Id}, product);
        }
        #endregion
        
        #region Put Requests
        /// <summary>
        /// Updating a product by using body.
        /// </summary>
        /// <param name="productForUpdate">All areas are required. Enter correct pattern and valid id.</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="404">Invalid Product Id</response>
        /// <response code="500">Server Error</response>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductForUpdateDto productForUpdate)
        {
            var product = _mapper.Map<Product>(productForUpdate);
            await _productRepository.UpdateAsync(product);
            _logger.LogInformation($"Product with id {product.Id} updated.");
            return Ok(product);
        }
        #endregion
        
        #region Delete Requests
        /// <summary>
        /// Deleting a product by using id of the product.
        /// </summary>
        /// <param name="id">Enter a valid id of a product</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Product Id</response>
        /// <response code="500">Server Error</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {

            await _orderRepository.UpdateProductRelatedOrders(id);
            //Delete product
            await _productRepository.DeleteAsync(id);
            //Logging and return
            _logger.LogInformation($"Product with id {id} deleted.");
            return Ok();
        }
        #endregion
        
    }