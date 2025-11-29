namespace template_backend.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Qty { get; set; }
    public decimal Price { get; set; }
}
