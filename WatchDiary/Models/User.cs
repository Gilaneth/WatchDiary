namespace WatchDiary.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
    public string UserEmail { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<WatchList> WatchList { get; set; } = [];
    public ICollection<Collection> Collections { get; set; } = [];
}
