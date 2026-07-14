using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("genre")]
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("genre_id")]
        public int GenreId { get; set; }
        [Column("genre_name")]
        [Required]
        public string Name { get; set; } = null!;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
