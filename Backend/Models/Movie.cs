namespace WatchDiary.Models;

public class Movie
{
    public int Id { get; set; }
    public int? TmdbId { get; set; }
    public string? ImdbId { get; set; }
    public decimal? ImdbRating { get; set; }
    public string MovieName { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public CategoryType Category { get; set; }
    public string? CoverUrl { get; set; }
    public string? Description { get; set; }

    public ICollection<Genre> Genres { get; set; } = [];
    public ICollection<Actor> Actors { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<WatchListItem> WatchListItems { get; set; } = [];
    public ICollection<Collection> Collections { get; set; } = [];
}
