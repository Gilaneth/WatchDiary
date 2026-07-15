using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;
using WatchDiary.Models.Dtos.Response;

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
            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
            .ToListAsync();

        var result = movies.Select(m => new MovieSummaryDto
                {
                MovieId = m.MovieId,
                MovieName = m.MovieName,
                ReleaseDate = m.ReleaseDate,
                Category = m.Category.ToString(),
                CoverUrl = m.CoverUrl,
                Description = m.Description,
                ImdbRating = m.ImdbRating,
                RtRating = m.RtRating,
                ShikimoriRating = m.ShikimoriRating,
                Genres = m.Genres.Select(g => g.GenreName).ToList(),
                Actors = m.MovieActors.Select(ma => ma.Actor.ActorName).ToList()
                });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
            .Include(m => m.Reviews)
            .Include(m => m.Collections)
            .FirstOrDefaultAsync(m => m.MovieId == id);

        if (movie is null) return NotFound();

        var result = new MovieDetailDto
        {
            MovieId = movie.MovieId,
            MovieName = movie.MovieName,
            ReleaseDate = movie.ReleaseDate,
            Category = movie.Category.ToString(),
            CoverUrl = movie.CoverUrl,
            Description = movie.Description,
            ImdbId = movie.ImdbId,
            ImdbRating = movie.ImdbRating,
            RottentomatoId = movie.RottentomatoId,
            RtRating = movie.RtRating,
            ShikimoriId = movie.ShikimoriId,
            ShikimoriRating = movie.ShikimoriRating,
            KinopoiskId = movie.KinopoiskId,
            Genres = movie.Genres.Select(g => new GenreSummaryDto
                    {
                    GenreId = g.GenreId,
                    GenreName = g.GenreName
                    }).ToList(),
            Actors = movie.MovieActors.Select(ma => new ActorSummaryDto
                    {
                    ActorId = ma.Actor.ActorId,
                    ActorName = ma.Actor.ActorName,
                    CharacterName = ma.CharacterName
                    }).ToList(),
            Reviews = movie.Reviews.Select(r => new ReviewSummaryDto
                    {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    UserId = r.UserId
                    }).ToList(),
            Collections = movie.Collections.Select(c => c.CollectionName).ToList()
        };

        return Ok(result);
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
