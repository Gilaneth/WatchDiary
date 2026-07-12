using Microsoft.AspNetCore.Mvc;
using WatchDiary.Services;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly TmdbService _tmdb;
    private readonly AppDbContext _db;

    public SearchController(TmdbService tmdb, AppDbContext db)
    {
        _tmdb = tmdb;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] string type = "multi")
    {
        if (string.IsNullOrWhiteSpace(q)) return BadRequest("Query cannot be empty.");

        var results = await _tmdb.SearchAsync(q, type);

        var mapped = results.Results.Select(r => new
                {
                TmdbId = r.Id,
                Title = r.Title ?? r.Name,
                Overview = r.Overview,
                ReleaseDate = r.Release_Date ?? r.First_Air_Date,
                PosterUrl = _tmdb.GetImageUrl(r.Poster_Path),
                Rating = r.Vote_Average,
                MediaType = r.Media_Type
                });

        return Ok(mapped);
    }

    [HttpPost("import/{tmdbId}")]
    public async Task<IActionResult> Import(int tmdbId, [FromQuery] string mediaType = "movie")
    {
        var existing = await _db.Movies.FirstOrDefaultAsync(m => m.TmdbId == tmdbId);
        if (existing is not null) return Conflict("Movie already exists in the catalogue.");

        var tmdbMovie = await _tmdb.GetByIdAsync(tmdbId, mediaType);
        if (tmdbMovie is null) return NotFound("Movie not found on TMDB.");

        var releaseDateStr = tmdbMovie.Release_Date ?? tmdbMovie.First_Air_Date;
        if (!DateOnly.TryParse(releaseDateStr, out var releaseDate))
            releaseDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var category = mediaType switch
        {
            "tv" => CategoryType.TvSeries,
            _ => CategoryType.Movie
        };

        var movie = new Movie
        {
            MovieName = tmdbMovie.Title ?? tmdbMovie.Name ?? "Unknown",
            Description = tmdbMovie.Overview,
            TmdbId = tmdbId,
            ReleaseDate = releaseDate,
            Category = category,
            CoverUrl = _tmdb.GetImageUrl(tmdbMovie.Poster_Path),
            ImdbRating = (decimal?)tmdbMovie.Vote_Average,
        };

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        return Ok(movie);
    }
}
