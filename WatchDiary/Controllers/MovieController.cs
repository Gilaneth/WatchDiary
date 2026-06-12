using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _db;

    public MovieController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .ToListAsync();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .Include(m => m.Reviews)
            .FirstOrDefaultAsync(m => m.MovieId == id);

        if (movie is null) return NotFound();
        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Movie movie)
    {
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = movie.MovieId }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Movie updated)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        movie.MovieName = updated.MovieName;
        movie.ReleaseDate = updated.ReleaseDate;
        movie.Category = updated.Category;
        movie.Description = updated.Description;
        movie.CoverUrl = updated.CoverUrl;
        movie.ImdbId = updated.ImdbId;
        movie.ImdbRating = updated.ImdbRating;
        movie.ShikimoriId = updated.ShikimoriId;
        movie.ShikimoriRating = updated.ShikimoriRating;
        movie.KinopoiskId = updated.KinopoiskId;
        movie.RottentomatoId = updated.RottentomatoId;
        movie.RtRating = updated.RtRating;

        await _db.SaveChangesAsync();
        return Ok(movie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
