namespace WatchDiary.Models.Dtos.Response;

public class MovieSummaryDto
{
    public int MovieId { get; set; }
    public string MovieName { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string Category { get; set; } = null!;
    public string? CoverUrl { get; set; }
    public string? Description { get; set; }
    public decimal? ImdbRating { get; set; }
    public decimal? RtRating { get; set; }
    public decimal? ShikimoriRating { get; set; }
    public List<string> Genres { get; set; } = [];
    public List<string> Actors { get; set; } = [];
}
