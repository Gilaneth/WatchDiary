namespace WatchDiary.Models;

public class Actor
{
    public int ActorId { get; set; }
    public string ActorName { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = [];
}
