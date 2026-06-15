using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;
using WatchDiary.Models.Dtos;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class GenreController : ControllerBase
{
    private readonly AppDbContext _db;

    public GenreController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _db.Genres.ToListAsync();
        return Ok(genres);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDto dto)
    {
        var genre = new Genre { GenreName = dto.GenreName };
        _db.Genres.Add(genre);
        await _db.SaveChangesAsync();
        return Ok(genre);
    }

    [HttpPost("movie")]
    public async Task<IActionResult> AddToMovie([FromBody] MovieGenreDto dto)
    {
        var movie = await _db.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.MovieId == dto.MovieId);
        if (movie is null) return NotFound("Movie not found.");

        var genre = await _db.Genres.FindAsync(dto.GenreId);
        if (genre is null) return NotFound("Genre not found.");

        if (movie.Genres.Any(g => g.GenreId == dto.GenreId))
            return Conflict("Genre already added to this movie.");

        movie.Genres.Add(genre);
        await _db.SaveChangesAsync();
        return Ok(movie);
    }

    [HttpDelete("movie")]
    public async Task<IActionResult> RemoveFromMovie([FromBody] MovieGenreDto dto)
    {
        var movie = await _db.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.MovieId == dto.MovieId);
        if (movie is null) return NotFound("Movie not found.");

        var genre = movie.Genres.FirstOrDefault(g => g.GenreId == dto.GenreId);
        if (genre is null) return NotFound("Genre not found on this movie.");

        movie.Genres.Remove(genre);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _db.Genres.FindAsync(id);
        if (genre is null) return NotFound();

        _db.Genres.Remove(genre);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
