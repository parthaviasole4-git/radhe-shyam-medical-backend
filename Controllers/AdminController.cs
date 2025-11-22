using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using template_backend.Data;

namespace template_backend.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminController(AppDbContext db) => _db = db;

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _db.Users.ToListAsync());
    }
}
