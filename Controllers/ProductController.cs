using Microsoft.AspNetCore.Mvc;
using template_backend.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetById(id);
        if (product == null) return NotFound("Product not found");

        return Ok(product);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> CreateMultiple(List<Product> products)
    {
        var created = await _productService.CreateMultiple(products);
        return Ok(created);
    }

}

