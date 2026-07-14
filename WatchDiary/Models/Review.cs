using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WatchDiary.Models
{
    [Table("review")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("review_id")]
        public int ReviewId { get; set; }
        [Column("rating")]
        [Required]
        public int Rating { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedAt { get; set; }
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
