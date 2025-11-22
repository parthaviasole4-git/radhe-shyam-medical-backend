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

    // GET ALL USERS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllAsync());
    }

    // GET ONE USER
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    // CREATE USER
    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var data = await _userService.CreateAsync(user);
        return Ok(data);
    }

    // UPDATE USER (FULL)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, User user)
    {
        if (id != user.Id) return BadRequest("ID mismatch");

        var ok = await _userService.UpdateAsync(user);
        return ok ? Ok("Updated") : NotFound();
    }

    // COMPLETE PROFILE (ONLY PROFILE FIELDS)
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
