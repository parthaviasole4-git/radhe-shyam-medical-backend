public class CartItemResponseDto
{
    public Guid Id { get; set; }
    public int Qty { get; set; }

    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImageUrl { get; set; }
    public decimal Price { get; set; }
}

