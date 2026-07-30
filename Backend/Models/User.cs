namespace WatchDiary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        //public string Password { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<WatchListItem> WatchListItems { get; set; } = new List<WatchListItem>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}