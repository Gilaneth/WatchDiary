using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;
using WatchDiary.Models.Dtos.Response;
using WatchDiary.Services;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly WikiService _wikiService;

    public MovieController(AppDbContext db, WikiService wikiService)
    {
        _db = db;
        _wikiService = wikiService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Actors)
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
            Actors = m.Actors.Select(a => a.ActorName).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var movie = await _db.Movies
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .Include(m => m.Reviews)
            .Include(m => m.Collections)
            .FirstOrDefaultAsync(m => m.MovieId == id);

        if (movie is null) return NotFound();

        if (string.IsNullOrEmpty(movie.FullPlot))
        {
            string wikiQuery = movie.MovieName;

            if (movie.Category == CategoryType.Movie)
            {
                wikiQuery += " (film)";
            }
            else
            {
                wikiQuery += " (TV series)";
            }

            var plot = await _wikiService.GetPlotAsync(wikiQuery);

            if (string.IsNullOrEmpty(plot) || plot.Contains("may refer to") || plot.Contains("refer to:"))
            {
                plot = await _wikiService.GetPlotAsync(movie.MovieName);
            }

            movie.FullPlot = plot;
            await _db.SaveChangesAsync();
        }

        var result = new MovieDetailDto
        {
            MovieId = movie.MovieId,
            MovieName = movie.MovieName,
            FullPlot = movie.FullPlot,
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
            Actors = movie.Actors.Select(a => new ActorSummaryDto
            {
                ActorId = a.ActorId,
                ActorName = a.ActorName
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