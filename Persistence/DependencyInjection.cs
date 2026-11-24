using Business.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;
using Persistence.Repositories;
using Persistence.Services;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure SQLite database connection.
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")));

        // Register repositories.
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IWatchlistItemRepository, WatchlistItemRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        // Configure HTTP client for TMDB API.
        services.AddHttpClient("TMDB", client =>
        {
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    configuration["TMDB:ReadAccessToken"]);
        });

        // Register TMDB service.
        services.AddScoped<TmdbService>();

        return services;
    }
}