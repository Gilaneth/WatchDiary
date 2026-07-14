using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    [Table("movie")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("movie_id")]
        public int MovieId { get; set; }
        [Column("imdb_id")]
        public string ImdbId { get; set; } = null!;
        [Column("shikimori_id")]
        [Required]
        public int ShikimoriId { get; set; }
        [Column("kinopoisk_id")]
        public int KinopoiskId { get; set; }
        [Column("rottentomato_id")]
        public string RottenTomatoId { get; set; } = null!;
        [Column("imdb_rating")]
        public decimal ImdbRating { get; set; }

        [Column("shikimori_rating")]
        public decimal ShikimoriRating { get; set; }
        [Column("rt_rating")]
        public decimal RottenTomatoRating { get; set; }
        [Column("movie_name")]
        public string Name { get; set; } = null!;
        [Column("release_date")]
        public DateTime ReleaseDate { get; set; }
        [Column("category")]
        public Category Category { get; set; }
        [Column("cover_url")]
        public string CoverUrl { get; set; } = null!;
        [Column("description")]
        public string Description { get; set; } = null!;
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<WatchListItem> WatchListItems { get; set; } = new List<WatchListItem>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
}
