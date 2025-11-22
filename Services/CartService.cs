using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;
public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddToCartAsync(CartItemRequestDto dto)
    {
        var cart = new CartItem
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            Qty = dto.Qty
        };

        _context.CartItems.Add(cart);
        await _context.SaveChangesAsync();

        return cart.Id;
    }

    public async Task<List<CartItemResponseDto>> GetCartAsync(Guid userId)
    {
        return await _context.CartItems
            .Where(x => x.UserId == userId)
            .Include(x => x.Product)
            .Select(x => new CartItemResponseDto
            {
                Id = x.Id,
                Qty = x.Qty,
                ProductId = x.Product.Id,
                ProductName = x.Product.Name,
                ProductImageUrl = x.Product.ImageUrl,
                Price = x.Product.Price
            })
            .ToListAsync();
    }

    public async Task<bool> RemoveFromCartAsync(Guid id)
    {
        var item = await _context.CartItems.FindAsync(id);
        if (item == null) return false;

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateQtyAsync(Guid id, int qty)
    {
        var item = await _context.CartItems.FindAsync(id);
        if (item == null) return false;

        item.Qty = qty;
        await _context.SaveChangesAsync();
        return true;
    }
}


