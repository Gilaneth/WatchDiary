using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("collections")]
    public class Collection
    {
        [Key]
        [Column("collection_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CollectionId { get; set; }
        [Column("collection_name")]
        public string? Name { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
