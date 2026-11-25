using Business.Services;
using Business.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        // Register business services.
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IWatchlistItemService, WatchlistItemService>();

        return services;
    }
}