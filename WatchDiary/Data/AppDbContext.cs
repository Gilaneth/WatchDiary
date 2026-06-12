using Microsoft.EntityFrameworkCore;
using WatchDiary.Models;

namespace WatchDiary.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<WatchList> WatchLists => Set<WatchList>();
    public DbSet<Collection> Collections => Set<Collection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Enums stored as strings in DB
        modelBuilder.HasPostgresEnum<WatchStatus>();
        modelBuilder.HasPostgresEnum<CategoryType>();

        // Many-to-many: Movie <-> Genre
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("movie_genre"));

        // Many-to-many: Movie <-> Actor
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity(j => j.ToTable("movie_actor"));

        // Many-to-many: Movie <-> Collection
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Collections)
            .WithMany(c => c.Movies)
            .UsingEntity(j => j.ToTable("movie_in_collection"));

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Movie>().ToTable("movie");
        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Actor>().ToTable("actor");
        modelBuilder.Entity<Review>().ToTable("review");
        modelBuilder.Entity<WatchList>().ToTable("watch_list");
        modelBuilder.Entity<Collection>().ToTable("collections");
    }
}
