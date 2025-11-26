namespace template_backend.Models;

public class CartItem
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public decimal Price { get; set; }

    public int Qty { get; set; }
}
