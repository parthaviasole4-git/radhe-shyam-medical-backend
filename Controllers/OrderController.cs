using Microsoft.AspNetCore.Mvc;
using template_backend.Services;

namespace template_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    // PLACE ORDER
    [HttpPost("place")]
    public async Task<IActionResult> PlaceOrder([FromBody] Guid userId)
    {
        try
        {
            var order = await _orderService.PlaceOrderAsync(userId);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET USER ORDERS
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }

    // GET ORDER DETAILS
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var order = await _orderService.GetOrderByIdAsync(orderId);
        return order == null ? NotFound() : Ok(order);
    }

    // ADMIN — GET ALL ORDERS
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    // UPDATE STATUS
    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateStatus(Guid orderId, [FromBody] string status)
    {
        var ok = await _orderService.UpdateStatusAsync(orderId, status);
        return ok ? Ok(new { message = "Status updated" }) : NotFound();
    }

    // DELETE ORDER
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(Guid orderId)
    {
        var ok = await _orderService.DeleteAsync(orderId);
        return ok ? Ok(new { message = "Order deleted" }) : NotFound();
    }
}
