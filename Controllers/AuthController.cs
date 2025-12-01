using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using template_backend.Data;
using template_backend.DTO;
using template_backend.Models;
using template_backend.Services;

namespace template_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly OtpService _otpService;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext db, OtpService otpService, TokenService tokenService)
    {
        _db = db;
        _otpService = otpService;
        _tokenService = tokenService;
    }

    // ---------------------------------------------------
    // SEND OTP
    // ---------------------------------------------------
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier))
            return BadRequest(new { message = "Identifier (email) is required." });

        // Generate OTP
        string otp = new Random().Next(100000, 999999).ToString();

        // Store OTP in DB
        var otpEntry = new OtpEntry
        {
            Identifier = request.Identifier,
            Otp = otp,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        _db.OtpEntries.Add(otpEntry);
        await _db.SaveChangesAsync();

        // Send Email via Gmail SMTP
        await _otpService.SendOtpAsync(request.Identifier, otp);

        return Ok(new { message = "OTP sent successfully." });
    }

    // ---------------------------------------------------
    // VERIFY OTP
    // ---------------------------------------------------
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Identifier) || string.IsNullOrWhiteSpace(request.Code))
            return BadRequest(new { message = "Identifier and OTP code are required." });

        // --------------------------------------------------
        // 1. Load OTP fully to avoid streaming reader issues
        // --------------------------------------------------
        var otpList = await _db.OtpEntries
            .AsNoTracking()
            .Where(x => x.Identifier == request.Identifier && x.Otp == request.Code)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();   // <-- FIX 2: fully materialize results

        var otpRow = otpList.FirstOrDefault();

        if (otpRow == null)
            return Unauthorized(new { message = "Invalid OTP." });

        if (otpRow.ExpiresAt < DateTime.UtcNow)
            return Unauthorized(new { message = "OTP expired." });

        // --------------------------------------------------
        // 2. Query Users — same DbContext is SAFE now
        // --------------------------------------------------
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier);

        if (user == null)
        {
            user = new User
            {
                Identifier = request.Identifier,
                Role = "User",
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        // --------------------------------------------------
        // 3. Generate JWT
        // --------------------------------------------------
        string token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            token,
            user = new
            {
                user.Id,
                user.Identifier,
                user.Role,
                user.IsAdmin
            },
            message = "OTP verified."
        });
    }


}
