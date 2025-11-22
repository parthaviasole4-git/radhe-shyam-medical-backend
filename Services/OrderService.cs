using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;

namespace template_backend.Services;

public class OrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
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

        var order = new Order
        {
            UserId = userId,
            Status = "Pending",
            OrderDate = DateTime.UtcNow,
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
        return await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // GET SINGLE ORDER
    public async Task<Order?> GetOrderByIdAsync(Guid orderId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    // ADMIN: GET ALL ORDERS
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // UPDATE STATUS (Admin)
    public async Task<bool> UpdateStatusAsync(Guid orderId, string newStatus)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.Status = newStatus;
        await _db.SaveChangesAsync();
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
