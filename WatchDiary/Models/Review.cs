namespace WatchDiary.Models;

public class Review
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}
