using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.Models;
using template_backend.Models.DTOs;

namespace template_backend.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    // GET SINGLE USER
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    // COMPLETE PROFILE (PROFILE PAGE)
    public async Task<bool> CompleteProfileAsync(Guid userId, CompleteProfileDto dto)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return false;

        user.DisplayName = dto.DisplayName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;
        user.Address = dto.Address;

        await _db.SaveChangesAsync();
        return true;
    }

    // DELETE USER
    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}
