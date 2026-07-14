using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("watch_list_item")]
    public class WatchListItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("watch_list_id")]
        public int WatchListItemId { get; set; }
        [Column("status")]
        public WatchStatus WatchStatus { get; set; }
        [Column("added_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime AddedAt { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [Column("movie_id")]
        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie? Movie { get; set; }

    }
}