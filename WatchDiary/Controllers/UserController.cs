using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users
            .Include(u => u.Reviews)
            .Include(u => u.WatchList)
            .Include(u => u.Collections)
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User updated)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        user.Username = updated.Username;

        await _db.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
