namespace WatchDiary.Models;

public class WatchList
{
    public int WatchListId { get; set; }
    public WatchStatus Status { get; set; }
    public DateTime AddedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
}
