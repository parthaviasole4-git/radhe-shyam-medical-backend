using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.DTOs;
using template_backend.Models;

namespace template_backend.Services;

public class OrderService
{
    private readonly AppDbContext _db;
    private readonly OtpService _otpService;

    public OrderService(AppDbContext db, OtpService otpService)
    {
        _db = db;
        _otpService = otpService;
    }

    // PLACE ORDER
    public async Task<Order> PlaceOrderAsync(Guid userId)
    {
        var cartItems = await _db.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
            throw new Exception("Cart is empty.");

        // Calculate total amount
        var total = cartItems.Sum(c => c.Qty * c.Product!.Price);

        var order = new Order
        {
            UserId = userId,
            Status = "Order Placed",
            OrderDate = DateTime.UtcNow,
            TotalAmount = total,
            Items = cartItems.Select(c => new OrderItem
            {
                ProductId = c.ProductId,
                Qty = c.Qty,
                Price = c.Product!.Price
            }).ToList()
        };

        _db.Orders.Add(order);

        // Clear cart
        _db.CartItems.RemoveRange(cartItems);

        await _db.SaveChangesAsync();
        return order;
    }

    // GET USER ORDERS
    public async Task<List<Order>> GetUserOrdersAsync(Guid userId)
    {
        return await _db.Orders.Where(o => o.UserId == userId).Include(o => o.User).Include(o => o.Items).ThenInclude(i => i.Product).OrderByDescending(o => o.OrderDate).ToListAsync();
    }

    // GET USER ORDERS DTO
    public async Task<List<OrderDto>> GetUserOrdersDtoAsync(Guid userId)
    {
        return await _db.Orders.OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                Status = o.Status,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
            })
            .ToListAsync();
    }

    // GET SINGLE ORDER
    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    // ADMIN: GET ALL ORDERS
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // Mark Out For Delivery
    public async Task<bool> MarkOutForDelivery(Guid orderId)
    {
        var order = await _db.Orders
            .Include(o => o.User)   // <-- IMPORTANT
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return false;

        string otp = new Random().Next(100000, 999999).ToString();

        order.DeliveryOtp = otp;
        order.DeliveryOtpExpiresAt = DateTime.UtcNow.AddDays(1);
        order.Status = "Out for Delivery";

        // Send OTP to customer (use email, not UserId)
        await _otpService.SendOtpAsync(order.User.Email, otp);

        await _db.SaveChangesAsync();
        return true;
    }


    // RESEND DELIVERY OTP
    public async Task<bool> ResendDeliveryOtpAsync(Guid orderId)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null) return false;

        string otp = new Random().Next(100000, 999999).ToString();
        order.DeliveryOtp = otp;
        order.DeliveryOtpExpiresAt = DateTime.UtcNow.AddDays(1);

        // send OTP to User Email
        await _otpService.SendOtpAsync(order.User.Email, otp);

        await _db.SaveChangesAsync();
        return true;
    }


    // VERIFY DELIVERY OTP
    public async Task<bool> VerifyDeliveryOtpAsync(Guid orderId, string otp)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null) return false;

        if (order.DeliveryOtp != otp)
            return false;

        if (order.DeliveryOtpExpiresAt < DateTime.UtcNow)
            return false;

        order.Status = "Delivered";
        await _db.SaveChangesAsync();
        // Delivery Verified
        return true;
    }

    // DELETE ORDER
    public async Task<bool> DeleteAsync(Guid orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null) return false;

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();
        return true;
    }

}
