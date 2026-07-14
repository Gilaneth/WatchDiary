using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("actor")]
    public class Actor
    {
        [Key]
        [Column("actor_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActorId { get; set; }
        [Column("actor_name")]
        [Required] 
        public string Name { get; set; } = null!;
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
