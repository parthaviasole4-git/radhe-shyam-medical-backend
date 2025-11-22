using template_backend.Models;

public interface IProductService
{
    Task<Product?> GetById(Guid id);
    Task<List<Product>> GetAll();
    Task<Product> Create(Product product);
    Task<List<Product>> CreateMultiple(List<Product> products);

}