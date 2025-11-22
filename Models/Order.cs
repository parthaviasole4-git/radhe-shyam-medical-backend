namespace template_backend.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Pending";

    public decimal TotalAmount { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
