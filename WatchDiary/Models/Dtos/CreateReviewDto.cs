namespace WatchDiary.Models.Dtos;

public class CreateReviewDto
{
    public int MovieId { get; set; }
    public int Rating { get; set; }
    public string? Description { get; set; }
}
