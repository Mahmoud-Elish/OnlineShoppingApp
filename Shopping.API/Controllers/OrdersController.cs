using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.Interfaces;
using Shopping.Shared;
using System.Security.Claims;

namespace Shopping.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : AppControllerBase
{
    private readonly IOrderRepository _orderRepo;

    public OrdersController(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(AddOrderDto orderDto)
    {
        try
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderRepo.CreateOrderAsync(UserId, orderDto);
            return FromOperationResult(result);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        try
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var order = await _orderRepo.GetOrderAsync(orderId, UserId);

            return Ok(order);
        }
        catch (Exception)
        {

            throw;
        }
       
    }

    [HttpGet]
    public async Task<IActionResult> GetUserOrders()
    {
        try
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _orderRepo.GetUserOrdersAsync(UserId);
            return Ok(orders);
        }
        catch (Exception)
        {

            throw;
        }      
    }

    [HttpPost("{orderId}/close")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CloseOrder(int orderId)
    {
        try
        {
            var result = await _orderRepo.CloseOrderAsync(orderId);
            return FromOperationResult(result);
        }
        catch (Exception)
        {

            throw;
        }        
    }
}
