using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] CartItemRequestDto dto)
    {
        var id = await _cartService.AddToCartAsync(dto);
        return Ok(new { id });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        var list = await _cartService.GetCartAsync(userId);
        return Ok(list);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var result = await _cartService.RemoveFromCartAsync(id);
        return result ? Ok() : NotFound();
    }

    [HttpPut("{id}/{qty}")]
    public async Task<IActionResult> UpdateQty(Guid id, int qty)
    {
        var result = await _cartService.UpdateQtyAsync(id, qty);
        return result ? Ok() : NotFound();
    }
}
