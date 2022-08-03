using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTO;
using OrderService.Application.Interfaces;
using OrderService.Application.Models;

namespace OrderServiceClean.API.Controllers;

[ApiController]
[Route("api/[controller]/")]
//[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderRequestService;

    public OrderController(IOrderService orderRequestService)
    {
        _orderRequestService = orderRequestService;
    }


    #region Get Requests

        /// <summary>
        /// List all order
        /// </summary>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        [HttpGet]
        //[ResponseCache(Duration = 120)]
        public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
        {
            var orders = await _orderRequestService.GetPagedOrders(requestParameters,HttpContext);
            return Ok(orders);
        }
        
        /// <summary>
        /// Fetch a order with giving id
        /// </summary>
        ///<param name="id">Id of specific order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="404">Invalid Order Id</response>
        /// <response code="500">Server Error</response>
        /// <response code="400">Invalid Format Error</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var order = await _orderRequestService.GetById(id);
            return Ok(order);
        }

        /// <summary>
        /// Fetch a orders of the given customer
        /// </summary>
        ///<param name="customerId">Id of specific customer</param>
        ///<param name="requestParameters">Paging params if needed</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer Id or No Order With The Customer</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersOfCustomers(string customerId,[FromQuery] RequestParameters requestParameters)
        {
            var orders =await _orderRequestService.GetOrdersOfCustomersPaged(customerId, requestParameters, HttpContext);
            return Ok(orders);
        }
        #endregion
        #region Post Requests

        /// <summary>
        /// Creating new order by using body.
        /// </summary>
        /// <param name="newOrder">All areas are required</param>
        /// <response code="201">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer or Product Id</response>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderForCreationDto newOrder)
        {
            var orderId =await _orderRequestService.CreateOrder(newOrder,HttpContext);
            return Ok(orderId);
        }
        #endregion
        #region Put Requests
        /// <summary>
        /// Updating an order by using body.
        /// </summary>
        /// <param name="newOrder">All areas are required. Enter correct pattern and valid id.</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Order or Product Id</response>
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderForUpdateDto newOrder)
        {
            var order =await _orderRequestService.UpdateOrder(newOrder);
            return Ok(order);
        }

        /// <summary>
        /// Updating the status of an order by using body.
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <param name="newStatus">New status of the order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Validation Failed or Invalid Request Body</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid OrderId</response>
        [HttpPut("status/{id}/{newStatus}")]
        public async Task<IActionResult> UpdateStatus(string id, string newStatus)
        {
            await _orderRequestService.UpdateStatus(id, newStatus);
            return Ok();
        }
        #endregion
        #region Delete Requests
        /// <summary>
        /// Deleting a order by using id of the order.
        /// </summary>
        /// <param name="id">Enter a valid id of a order</param>
        /// <response code="200">Successful Response</response>
        /// <response code="400">Invalid Id Format Error</response>
        /// <response code="404">Invalid Order Id </response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            await _orderRequestService.DeleteOrder(id);
            return Ok();
        }

        /// <summary>
        /// Deleting all orders of a customer with given id.
        /// </summary>
        /// <param name="customerId">Enter a valid id of a customer</param>
        /// <response code="200">Successful Response</response>
        /// <response code="500">Server Error</response>
        /// <response code="404">Invalid Customer Id or No Order With The Customer</response>
        /// <response code="400">Invalid Id Format Error</response>
        [HttpDelete("customer/{customerId}")]
        public async Task<IActionResult> DeleteOrderOfCustomer(string customerId)
        {
            await _orderRequestService.DeleteOrderOfCustomer(customerId);
            return Ok();
        }
        #endregion
    
}