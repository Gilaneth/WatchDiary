using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
using WatchDiary.Extensions;
using WatchDiary.Models;
using WatchDiary.Models.Dtos;

namespace WatchDiary.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReviewController(AppDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpGet("movie/{movieId}")]
    public async Task<IActionResult> GetByMovie(int movieId)
    {
        var reviews = await _db.Reviews
            .Include(r => r.User)
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var reviews = await _db.Reviews
            .Include(r => r.Movie)
            .Where(r => r.UserId == userId)
            .ToListAsync();
        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _db.Reviews
            .Include(r => r.User)
            .Include(r => r.Movie)
            .FirstOrDefaultAsync(r => r.ReviewId == id);

        if (review is null) return NotFound();
        return Ok(review);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewDto dto)
    {
        var userId = User.GetUserId();
        var movieExists = await _db.Movies.AnyAsync(m => m.MovieId == dto.MovieId);
        if (!movieExists) return BadRequest("Movie not found.");

        var review = new Review
        {
            UserId = userId,
            MovieId = dto.MovieId,
            Rating = dto.Rating,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = review.ReviewId }, review);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Review updated)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review is null) return NotFound();
        if (review.UserId != User.GetUserId()) return Forbid();

        review.Rating = updated.Rating;
        review.Description = updated.Description;
        review.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(review);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review is null) return NotFound();
        if (review.UserId != User.GetUserId()) return Forbid();

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
