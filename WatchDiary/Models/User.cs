using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Column("username")]
        [Required]
        public string Username { get; set; } = null!;

        [Column("user_password")]
        [Required]
        public string Password { get; set; } = null!;

        [Column("user_email")]
        [Required]
        public string UserEmail { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<WatchListItem> WatchListItems { get; set; } = new List<WatchListItem>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}