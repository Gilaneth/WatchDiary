using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WatchDiary.Models;


namespace WatchDiary.Data
{

    public class WatchDiaryDbContext : DbContext
    {
        public WatchDiaryDbContext(DbContextOptions<WatchDiaryDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WatchListItem> WatchListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>();
            modelBuilder.Entity<Collection>();
            modelBuilder.Entity<Genre>();
            modelBuilder.Entity<Movie>();
            modelBuilder.Entity<Review>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<WatchListItem>();

            modelBuilder.HasPostgresEnum<Category>("public", "category_type");
            modelBuilder.HasPostgresEnum<WatchStatus>("public", "watch_status");

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies)
                .UsingEntity(j => j.ToTable("movie_actor"));

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity(j => j.ToTable("movie_genre"));

            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Collections)
                .WithMany(c => c.Movies)
                .UsingEntity(j => j.ToTable("movie_in_collection"));
        }   





    }
}
