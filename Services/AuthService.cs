using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;

namespace template_backend.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> GetOrCreateUser(string identifier)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Identifier == identifier);

        if (user == null)
        {
            user = new User { Identifier = identifier };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        return user;
    }
}
