public interface ICartService
{
    Task<Guid> AddToCartAsync(CartItemRequestDto dto);
    Task<List<CartItemResponseDto>> GetCartAsync(Guid userId);
    Task<bool> RemoveFromCartAsync(Guid id);
    Task<bool> UpdateQtyAsync(Guid id, int qty);
}
