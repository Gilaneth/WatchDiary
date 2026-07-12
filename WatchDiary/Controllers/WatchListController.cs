using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Extensions;
using WatchDiary.Models;
using WatchDiary.Models.Dtos;

namespace WatchDiary.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WatchListController : ControllerBase
{
    private readonly AppDbContext _db;

    public WatchListController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        if (userId != User.GetUserId()) return Forbid();

        var items = await _db.WatchLists
            .Include(w => w.Movie)
            .Where(w => w.UserId == userId)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("user/{userId}/status/{status}")]
    public async Task<IActionResult> GetByStatus(int userId, WatchStatus status)
    {
        if (userId != User.GetUserId()) return Forbid();

        var items = await _db.WatchLists
            .Include(w => w.Movie)
            .Where(w => w.UserId == userId && w.Status == status)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateWatchListDto dto)
    {
        var userId = User.GetUserId();
        var movieExists = await _db.Movies.AnyAsync(m => m.MovieId == dto.MovieId);
        if (!movieExists) return BadRequest("Movie not found.");

        var existing = await _db.WatchLists
            .FirstOrDefaultAsync(w => w.UserId == userId && w.MovieId == dto.MovieId);
        if (existing is not null) return Conflict("Movie already in watchlist.");

        var item = new WatchList
        {
            UserId = userId,
            MovieId = dto.MovieId,
            Status = dto.Status,
            AddedAt = DateTime.UtcNow
        };

        _db.WatchLists.Add(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] WatchStatus status)
    {
        var item = await _db.WatchLists.FindAsync(id);
        if (item is null) return NotFound();
        if (item.UserId != User.GetUserId()) return Forbid();

        item.Status = status;
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var item = await _db.WatchLists.FindAsync(id);
        if (item is null) return NotFound();
        if (item.UserId != User.GetUserId()) return Forbid();

        _db.WatchLists.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
