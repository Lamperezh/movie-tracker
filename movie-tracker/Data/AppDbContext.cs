using Microsoft.EntityFrameworkCore;
using movie_tracker.Models;

namespace movie_tracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка User
        modelBuilder.Entity<User>(e =>
        {
            e.Property(x => x.Username).HasMaxLength(50).IsRequired();
            e.Property(x => x.Email).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });

        // Настройка Movie
        modelBuilder.Entity<Movie>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Genre).HasMaxLength(100);
            e.Property(x => x.PosterUrl).HasMaxLength(500);
        });

        // Настройка Actor
        modelBuilder.Entity<Actor>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Country).HasMaxLength(100);
        });

        // Связь Many-to-Many: Movie <-> Actor (EF Core создаст таблицу автоматически)
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies);

        // Настройка WatchlistItem
        modelBuilder.Entity<WatchlistItem>(e =>
        {
            e.HasIndex(x => new { x.UserId, x.MovieId }).IsUnique();

            e.HasOne(x => x.User)
                .WithMany(x => x.WatchlistItems)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Movie)
                .WithMany(x => x.WatchlistItems)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Настройка Review
        modelBuilder.Entity<Review>(e =>
        {
            e.HasIndex(x => new { x.UserId, x.MovieId }).IsUnique();
            e.Property(x => x.Comment).HasMaxLength(2000);

            e.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Movie)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}