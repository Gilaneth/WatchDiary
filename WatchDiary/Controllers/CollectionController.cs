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
public class CollectionController : ControllerBase
{
    private readonly AppDbContext _db;

    public CollectionController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        if (userId != User.GetUserId()) return Forbid();

        var collections = await _db.Collections
            .Include(c => c.Movies)
            .Where(c => c.UserId == userId)
            .ToListAsync();
        return Ok(collections);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var collection = await _db.Collections
            .Include(c => c.Movies)
            .FirstOrDefaultAsync(c => c.CollectionId == id);

        if (collection is null) return NotFound();
        if (collection.UserId != User.GetUserId()) return Forbid();

        return Ok(collection);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCollectionDto dto)
    {
        var collection = new Collection
        {
            UserId = User.GetUserId(),
            CollectionName = dto.CollectionName
        };

        _db.Collections.Add(collection);
        await _db.SaveChangesAsync();
        return Ok(collection);
    }

    [HttpPost("movie")]
    public async Task<IActionResult> AddMovie([FromBody] CollectionMovieDto dto)
    {
        var collection = await _db.Collections.Include(c => c.Movies).FirstOrDefaultAsync(c => c.CollectionId == dto.CollectionId);
        if (collection is null) return NotFound("Collection not found.");
        if (collection.UserId != User.GetUserId()) return Forbid();

        var movie = await _db.Movies.FindAsync(dto.MovieId);
        if (movie is null) return NotFound("Movie not found.");

        if (collection.Movies.Any(m => m.MovieId == dto.MovieId))
            return Conflict("Movie already in collection.");

        collection.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return Ok(collection);
    }

    [HttpDelete("movie")]
    public async Task<IActionResult> RemoveMovie([FromBody] CollectionMovieDto dto)
    {
        var collection = await _db.Collections.Include(c => c.Movies).FirstOrDefaultAsync(c => c.CollectionId == dto.CollectionId);
        if (collection is null) return NotFound("Collection not found.");
        if (collection.UserId != User.GetUserId()) return Forbid();

        var movie = collection.Movies.FirstOrDefault(m => m.MovieId == dto.MovieId);
        if (movie is null) return NotFound("Movie not found in collection.");

        collection.Movies.Remove(movie);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var collection = await _db.Collections.FindAsync(id);
        if (collection is null) return NotFound();
        if (collection.UserId != User.GetUserId()) return Forbid();

        _db.Collections.Remove(collection);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
