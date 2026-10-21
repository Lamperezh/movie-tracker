using System.Text.Json;
using movie_tracker.DTOs.Tmdb;

namespace movie_tracker.Services;

public class TmdbService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TmdbService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TmdbPopularMoviesResponse?> GetPopularMoviesAsync(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"movie/popular?language=uk-UA&page={page}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TmdbPopularMoviesResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}