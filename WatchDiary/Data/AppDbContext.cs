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
        modelBuilder.HasPostgresEnum<CategoryType>("public", "category_type");
        modelBuilder.HasPostgresEnum<WatchStatus>("public", "watch_status");

        // Table names
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Movie>().ToTable("movie");
        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Actor>().ToTable("actor");
        modelBuilder.Entity<Review>().ToTable("review");
        modelBuilder.Entity<WatchList>().ToTable("watch_list");
        modelBuilder.Entity<Collection>().ToTable("collections");

        // Column mappings
        modelBuilder.Entity<User>(e => {
                e.Property(u => u.UserId).HasColumnName("user_id");
                e.Property(u => u.Username).HasColumnName("username");
                e.Property(u => u.UserPassword).HasColumnName("user_password");
                e.Property(u => u.UserEmail).HasColumnName("user_email");
                });

        modelBuilder.Entity<Movie>(e => {
                e.Property(m => m.MovieId).HasColumnName("movie_id");
                e.Property(m => m.Category).HasColumnName("category");
                e.Property(m => m.MovieName).HasColumnName("movie_name");
                e.Property(m => m.ReleaseDate).HasColumnName("release_date");
                e.Property(m => m.CoverUrl).HasColumnName("cover_url");
                e.Property(m => m.Description).HasColumnName("description");
                e.Property(m => m.TmdbId).HasColumnName("tmdb_id");
                e.Property(m => m.ImdbId).HasColumnName("imdb_id");
                e.Property(m => m.ImdbRating).HasColumnName("imdb_rating");
                e.Property(m => m.ShikimoriId).HasColumnName("shikimori_id");
                e.Property(m => m.ShikimoriRating).HasColumnName("shikimori_rating");
                e.Property(m => m.KinopoiskId).HasColumnName("kinopoisk_id");
                e.Property(m => m.RottentomatoId).HasColumnName("rottentomato_id");
                e.Property(m => m.RtRating).HasColumnName("rt_rating");
                });

        modelBuilder.Entity<Genre>(e => {
                e.Property(g => g.GenreId).HasColumnName("genre_id");
                e.Property(g => g.GenreName).HasColumnName("genre_name");
                });

        modelBuilder.Entity<Actor>(e => {
                e.Property(a => a.ActorId).HasColumnName("actor_id");
                e.Property(a => a.ActorName).HasColumnName("actor_name");
                });

        modelBuilder.Entity<Review>(e => {
                e.Property(r => r.ReviewId).HasColumnName("review_id");
                e.Property(r => r.Rating).HasColumnName("rating");
                e.Property(r => r.Description).HasColumnName("description");
                e.Property(r => r.CreatedAt).HasColumnName("created_at");
                e.Property(r => r.UpdatedAt).HasColumnName("updated_at");
                e.Property(r => r.UserId).HasColumnName("user_id");
                e.Property(r => r.MovieId).HasColumnName("movie_id");
                });

        modelBuilder.Entity<WatchList>(e => {
                e.Property(w => w.WatchListId).HasColumnName("watch_list_id");
                e.Property(w => w.Status).HasColumnName("status");
                e.Property(w => w.AddedAt).HasColumnName("added_at");
                e.Property(w => w.UserId).HasColumnName("user_id");
                e.Property(w => w.MovieId).HasColumnName("movie_id");
                });

        modelBuilder.Entity<Collection>(e => {
                e.Property(c => c.CollectionId).HasColumnName("collection_id");
                e.Property(c => c.CollectionName).HasColumnName("collection_name");
                e.Property(c => c.UserId).HasColumnName("user_id");
                });

        // Many-to-many: Movie <-> Genre
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => {
                    j.ToTable("movie_genre");
                    j.Property("MoviesMovieId").HasColumnName("movie_id");
                    j.Property("GenresGenreId").HasColumnName("genre_id");
                    });

        // Many-to-many: Movie <-> Actor
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies)
            .UsingEntity(j => {
                    j.ToTable("movie_actor");
                    j.Property("MoviesMovieId").HasColumnName("movie_id");
                    j.Property("ActorsActorId").HasColumnName("actor_id");
                    });

        // Many-to-many: Movie <-> Collection
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Collections)
            .WithMany(c => c.Movies)
            .UsingEntity(j => {
                    j.ToTable("movie_in_collection");
                    j.Property("MoviesMovieId").HasColumnName("movie_id");
                    j.Property("CollectionsCollectionId").HasColumnName("collection_id");
                    });
    }
}
