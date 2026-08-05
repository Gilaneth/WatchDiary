namespace WatchDiary.Models;

public class Actor
{
    public int Id { get; set; }
    public string ActorName { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = [];
}
