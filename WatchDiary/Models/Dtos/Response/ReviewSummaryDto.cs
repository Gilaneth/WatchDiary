namespace WatchDiary.Models.Dtos.Response;

public class ReviewSummaryDto
{
    public int ReviewId { get; set; }
    public int Rating { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}
