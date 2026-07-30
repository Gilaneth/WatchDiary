namespace WatchDiary.Models;

public class Collection
{
    public int CollectionId { get; set; }
    public string CollectionName { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = [];
}
