namespace WatchDiary.Models.Dtos.Response;

public class MovieDetailDto
{
    public int MovieId { get; set; }
    public string MovieName { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public string Category { get; set; } = null!;
    public string? CoverUrl { get; set; }
    public string? Description { get; set; }
    public string? ImdbId { get; set; }
    public decimal? ImdbRating { get; set; }
    public string? RottentomatoId { get; set; }
    public decimal? RtRating { get; set; }
    public int? ShikimoriId { get; set; }
    public decimal? ShikimoriRating { get; set; }
    public int? KinopoiskId { get; set; }
    public List<GenreSummaryDto> Genres { get; set; } = [];
    public List<ActorSummaryDto> Actors { get; set; } = [];
    public List<ReviewSummaryDto> Reviews { get; set; } = [];
    public List<string> Collections { get; set; } = [];
}
