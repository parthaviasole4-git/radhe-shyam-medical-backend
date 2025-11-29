using Microsoft.AspNetCore.Mvc;
using template_backend.DTO;
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

    // UPDATE STATUS + GENERATE OTP
    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> MarkOutForDelivery(Guid orderId)
    {
        var ok = await _orderService.MarkOutForDelivery(orderId);
        return ok ? Ok(new { message = "Status updated" }) : NotFound();
    }

    // RESEND DELIVERY OTP
    [HttpPost("resend-otp/{orderId}")]
    public async Task<IActionResult> ResendOtp(Guid orderId)
    {
        await _orderService.ResendDeliveryOtpAsync(orderId);
        return Ok(new { message = "OTP resent successfully" });
    }

    // VERIFY DELIVERY OTP
    [HttpPost("verify-delivery-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] DeliveryOtpVerifyDto dto)
    {
        var ok = await _orderService.VerifyDeliveryOtpAsync(dto.OrderId, dto.Otp);
        return ok ? Ok(new { message = "Delivery verified" }) : BadRequest(new { message = "Invalid or expired OTP" });
    }

    // DELETE ORDER
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(Guid orderId)
    {
        var ok = await _orderService.DeleteAsync(orderId);
        return ok ? Ok(new { message = "Order deleted" }) : NotFound();
    }
}
