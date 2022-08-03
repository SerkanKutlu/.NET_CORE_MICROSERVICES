using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;

namespace OrderServiceClean.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
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
            var products = await _productService.GetProductsPaged(requestParameters, HttpContext);
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
            var product = await _productService.GetById(id);
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
            var productId =await _productService.CreateProduct(productForCreation, HttpContext);
            return Ok(productId);
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
            var product =await _productService.UpdateProduct(productForUpdate);
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

            await _productService.DeleteProduct(id);
            return Ok();
        }
        #endregion
        
    }