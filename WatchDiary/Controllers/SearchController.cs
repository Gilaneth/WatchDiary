using Microsoft.AspNetCore.Mvc;
using WatchDiary.Services;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Models;
using WatchDiary.Models.Dtos.Response;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly TmdbService _tmdb;
    private readonly AppDbContext _db;
    private readonly OmdbService _omdb;  

    public SearchController(TmdbService tmdb, AppDbContext db, OmdbService omdb)
    {
        _tmdb = tmdb;
        _db = db;
        _omdb = omdb;
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
        if (existing is not null) return Conflict("Movie/Show already exists.");

        var isMovie = mediaType.ToLower() == "movie";
        var tmdbData = isMovie
            ? await _tmdb.GetMovieDetailsAsync(tmdbId)
            : await _tmdb.GetTvShowDetailsAsync(tmdbId);

        if (tmdbData is null) return NotFound("Not found on TMDB.");

        var movie = new Movie
        {
            TmdbId = tmdbData.Id,
            MovieName = tmdbData.Title ?? tmdbData.Name ?? "Unknown",
            Description = tmdbData.Overview,
            Category = isMovie ? CategoryType.Movie : CategoryType.TvSeries,
            CoverUrl = _tmdb.GetImageUrl(tmdbData.PosterPath),
            ImdbId = tmdbData.ExternalIds?.ImdbId
        };

        var dateString = tmdbData.ReleaseDate ?? tmdbData.FirstAirDate;
        movie.ReleaseDate = DateTime.TryParse(dateString, out var date) ? DateOnly.FromDateTime(date) : DateOnly.FromDateTime(DateTime.UtcNow);

        if (!string.IsNullOrEmpty(movie.ImdbId))
        {
            var (imdbRating, rtRating) = await _omdb.GetRatingsAsync(movie.ImdbId);
            movie.ImdbRating = imdbRating;
            movie.RtRating = rtRating;
        }
        else
        {
            movie.ImdbRating = (decimal?)tmdbData.VoteAverage;
        }

        movie.Genres = await MapGenres(tmdbData.Genres);

        var actorsList = tmdbData.AggregateCredits?.Cast?.Count > 0 
            ? tmdbData.AggregateCredits.Cast 
            : tmdbData.Credits?.Cast;

        movie.Actors = await MapActors(actorsList);

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();

        return Ok(movie);
    }

    private async Task<List<Genre>> MapGenres(List<TmdbGenreDto>? genres)
    {
        var list = new List<Genre>();
        if (genres == null) return list;
        foreach (var g in genres)
        {
            var existing = await _db.Genres.FirstOrDefaultAsync(x => x.GenreName == g.Name);
            list.Add(existing ?? new Genre { GenreName = g.Name });
        }
        return list;
    }

    private async Task<List<Actor>> MapActors(List<TmdbCastDto>? cast)
    {
        var list = new List<Actor>();
        if (cast == null) return list;
        foreach (var c in cast.Take(5))
        {
            var existing = await _db.Actors.FirstOrDefaultAsync(x => x.ActorName == c.Name);
            list.Add(existing ?? new Actor { ActorName = c.Name });
        }
        return list;
    }
}