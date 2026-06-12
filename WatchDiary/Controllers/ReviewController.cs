using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;
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

    [HttpGet("movie/{movieId}")]
    public async Task<IActionResult> GetByMovie(int movieId)
    {
        var reviews = await _db.Reviews
            .Include(r => r.User)
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
        return Ok(reviews);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var reviews = await _db.Reviews
            .Include(r => r.Movie)
            .Where(r => r.UserId == userId)
            .ToListAsync();
        return Ok(reviews);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewDto dto)
    {
        var userExists = await _db.Users.AnyAsync(u => u.UserId == dto.UserId);
        var movieExists = await _db.Movies.AnyAsync(m => m.MovieId == dto.MovieId);

        if (!userExists) return BadRequest("User not found.");
        if (!movieExists) return BadRequest("Movie not found.");

        var review = new Review
        {
            UserId = dto.UserId,
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Review updated)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review is null) return NotFound();

        review.Rating = updated.Rating;
        review.Description = updated.Description;
        review.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(review);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review is null) return NotFound();

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
