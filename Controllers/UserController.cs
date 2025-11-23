using Microsoft.AspNetCore.Mvc;
using template_backend.Models;
using template_backend.Models.DTOs;
using template_backend.Services;

namespace template_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // GET ONE USER
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    // COMPLETE PROFILE
    [HttpPut("complete-profile/{userId}")]
    public async Task<IActionResult> CompleteProfile(Guid userId, [FromBody] CompleteProfileDto dto)
    {
        var ok = await _userService.CompleteProfileAsync(userId, dto);

        return ok
            ? Ok(new { message = "Profile updated" })
            : NotFound(new { message = "User not found" });
    }

    // DELETE USER
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _userService.DeleteAsync(id);
        return ok ? Ok("Deleted") : NotFound();
    }
}
