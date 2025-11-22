using System.Text.Json.Serialization;
using template_backend.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    [JsonIgnore] // 
    public Order Order { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public int Qty { get; set; }

    public decimal Price { get; set; }

    public decimal SubTotal => Qty * Price;
}
