using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Extensions;
using WatchDiary.Models;
using WatchDiary.Models.Dtos;
using WatchDiary.Models.Dtos.Response;
using WatchDiary.Services;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher<User> _hasher;
    private readonly TokenService _tokenService;

    public UserController(AppDbContext db, IPasswordHasher<User> hasher, TokenService tokenService)
    {
        _db = db;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .Select(u => new UserSummaryDto
                    {
                    UserId = u.UserId,
                    Username = u.Username,
                    UserEmail = u.UserEmail
                    })
        .ToListAsync();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null) return NotFound();

        return Ok(new UserSummaryDto
                {
                UserId = user.UserId,
                Username = user.Username,
                UserEmail = user.UserEmail
                });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            UserEmail = dto.UserEmail,
            UserPassword = string.Empty
        };
        user.UserPassword = _hasher.HashPassword(user, dto.UserPassword);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new UserSummaryDto
                {
                UserId = user.UserId,
                Username = user.Username,
                UserEmail = user.UserEmail
                });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
        if (user is null) return Unauthorized("Invalid username or password.");

        var result = _hasher.VerifyHashedPassword(user, user.UserPassword, dto.Password);
        if (result == PasswordVerificationResult.Failed) return Unauthorized("Invalid username or password.");

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User updated)
    {
        if (id != User.GetUserId()) return Forbid();

        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        user.Username = updated.Username;
        await _db.SaveChangesAsync();

        return Ok(new UserSummaryDto
                {
                UserId = user.UserId,
                Username = user.Username,
                UserEmail = user.UserEmail
                });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id != User.GetUserId()) return Forbid();

        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
