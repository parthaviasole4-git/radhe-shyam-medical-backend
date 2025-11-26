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
        // Check if product already exists in user's cart
        var existing = await _context.CartItems
            .FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.ProductId == dto.ProductId);

        // If already exists → increase qty
        if (existing != null)
        {
            existing.Qty += dto.Qty;

            await _context.SaveChangesAsync();
            return existing.Id;   // return existing cart item id
        }

        // Otherwise create new cart entry
        var cart = new CartItem
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            Qty = dto.Qty,
            Price = dto.Price
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


