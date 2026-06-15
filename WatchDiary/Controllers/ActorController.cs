using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;
using WatchDiary.Models.Dtos;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class ActorController : ControllerBase
{
    private readonly AppDbContext _db;

    public ActorController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var actors = await _db.Actors.ToListAsync();
        return Ok(actors);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateActorDto dto)
    {
        var actor = new Actor { ActorName = dto.ActorName };
        _db.Actors.Add(actor);
        await _db.SaveChangesAsync();
        return Ok(actor);
    }

    [HttpPost("movie")]
    public async Task<IActionResult> AddToMovie([FromBody] MovieActorDto dto)
    {
        var movie = await _db.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.MovieId == dto.MovieId);
        if (movie is null) return NotFound("Movie not found.");

        var actor = await _db.Actors.FindAsync(dto.ActorId);
        if (actor is null) return NotFound("Actor not found.");

        if (movie.Actors.Any(a => a.ActorId == dto.ActorId))
            return Conflict("Actor already added to this movie.");

        movie.Actors.Add(actor);
        await _db.SaveChangesAsync();
        return Ok(movie);
    }

    [HttpDelete("movie")]
    public async Task<IActionResult> RemoveFromMovie([FromBody] MovieActorDto dto)
    {
        var movie = await _db.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.MovieId == dto.MovieId);
        if (movie is null) return NotFound("Movie not found.");

        var actor = movie.Actors.FirstOrDefault(a => a.ActorId == dto.ActorId);
        if (actor is null) return NotFound("Actor not found on this movie.");

        movie.Actors.Remove(actor);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var actor = await _db.Actors.FindAsync(id);
        if (actor is null) return NotFound();

        _db.Actors.Remove(actor);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
