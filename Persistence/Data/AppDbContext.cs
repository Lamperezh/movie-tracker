using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Business.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Media> Media => Set<Media>();
    public DbSet<WatchlistItem> WatchlistItems => Set<WatchlistItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var ostLinksConverter = new ValueConverter<List<string>, string>(
            links => JsonSerializer.Serialize(links, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<List<string>>(
                json, (JsonSerializerOptions?)null) ?? new List<string>());

        var ostLinksComparer = new ValueComparer<List<string>>(
            (a, b) => a != null && b != null && a.SequenceEqual(b),
            links => links.Aggregate(
                0,
                (hash, link) => HashCode.Combine(hash, link.GetHashCode())),
            links => links.ToList());

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Media>(e =>
        {
            e.Property(x => x.OstLinks)
                .HasConversion(ostLinksConverter)
                .Metadata.SetValueComparer(ostLinksComparer);

            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt).IsRequired();

            e.HasIndex(x => new { x.TmdbId, x.Type }).IsUnique();
        });

        modelBuilder.Entity<WatchlistItem>(e =>
        {
            e.HasIndex(x => new { x.UserId, x.TmdbId, x.Type }).IsUnique();

            e.HasOne(x => x.User)
                .WithMany(x => x.WatchlistItems)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Review>(e =>
        {
            e.HasIndex(x => new { x.UserId, x.TmdbId, x.Type }).IsUnique();

            e.HasOne(x => x.User)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            e.Property(x => x.Rating).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
        });
    }
}