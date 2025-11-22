namespace template_backend.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";  // "diabetes", "painkiller"
    public string Brand { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public decimal Price { get; set; }
    public decimal MRP { get; set; }
    public string Description { get; set; } = "";
}
