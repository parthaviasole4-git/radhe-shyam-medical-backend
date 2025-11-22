using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetById(Guid id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product> Create(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> CreateMultiple(List<Product> products)
    {
        foreach (var p in products)
        {
            if (p.Id == Guid.Empty)
                p.Id = Guid.NewGuid(); // auto generate Guid if not set
        }

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        return products;
    }

}

