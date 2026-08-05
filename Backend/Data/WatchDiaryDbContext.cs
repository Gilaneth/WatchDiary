using Microsoft.EntityFrameworkCore;
using WatchDiary.Models;

namespace Backend.Data
{
    public class WatchDiaryDbContext : DbContext
    {
        public WatchDiaryDbContext(DbContextOptions<WatchDiaryDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<Collection> Collections => Set<Collection>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<WatchListItem> WatchListItems => Set<WatchListItem>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<CategoryType>().HaveConversion<string>();
            configurationBuilder.Properties<WatchStatus>().HaveConversion<string>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Username).IsUnique();
                e.HasIndex(u => u.UserEmail).IsUnique();
                e.HasMany(u => u.Reviews)
                .WithOne(r => r.User);

                e.Property(u => u.Username).HasMaxLength(50).IsRequired();
                e.Property(u => u.UserEmail).HasMaxLength(256).IsRequired();
            });
            modelBuilder.Entity<Actor>(e =>
            {
                e.HasIndex(a => a.ActorName).IsUnique();
            });
            modelBuilder.Entity<Collection>(e =>
            {
                e.HasIndex(c => new { c.UserId, c.CollectionName }).IsUnique();
                e.Property(c => c.CollectionName).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Movie>(e =>
            {
                e.HasIndex(m => m.ReleaseDate);
                e.HasIndex(m => m.ImdbRating);
                e.HasIndex(m => m.Category);
                e.HasIndex(m => new { m.Category, m.ImdbRating });

                e.HasMany(m => m.Actors)
                .WithMany(a => a.Movies);
                e.HasMany(m => m.Collections)
                .WithMany(c => c.Movies);
                e.HasMany(m => m.Genres)
                .WithMany(g => g.Movies);
                e.HasMany(m => m.Reviews)
                .WithOne(r => r.Movie);
                e.HasMany(m => m.WatchListItems)
                .WithOne(w => w.Movie);
            });

            modelBuilder.Entity<Genre>(e =>
            {
                e.HasIndex(g => g.GenreName).IsUnique();
            });

            modelBuilder.Entity<Review>(e => 
            {
                e.HasIndex(r=> new {r.UserId, r.MovieId}).IsUnique();
                e.HasIndex(r => r.CreatedAt);
            });

            modelBuilder.Entity<WatchListItem>(e =>
            {
                e.HasIndex(w => new { w.UserId, w.MovieId }).IsUnique();
                e.HasIndex(w => new { w.UserId, w.Status });
                e.HasIndex(w => w.AddedAt);
            });
        }
    }
}
