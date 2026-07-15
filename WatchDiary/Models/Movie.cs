namespace WatchDiary.Models;

public class Movie
{
    public int MovieId { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public int? ShikimoriId { get; set; }
    public int? KinopoiskId { get; set; }
    public string? RottentomatoId { get; set; }
    public decimal? ImdbRating { get; set; }
    public decimal? ShikimoriRating { get; set; }
    public decimal? RtRating { get; set; }
    public string MovieName { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public CategoryType Category { get; set; }
    public string? CoverUrl { get; set; }
    public string? Description { get; set; }

    public ICollection<Genre> Genres { get; set; } = [];
    public ICollection<MovieActor> MovieActors { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<WatchList> WatchList { get; set; } = [];
    public ICollection<Collection> Collections { get; set; } = [];
}
