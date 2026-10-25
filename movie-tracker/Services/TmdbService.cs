using System.Globalization;
using System.Text.Json;
using movie_tracker.DTOs.Tmdb;
using movie_tracker.DTOs.Tmdb.Movie;

namespace movie_tracker.Services;

public class TmdbService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TmdbService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<TmdbMovieResponseDto?> DiscoverMoviesAsync(
        TmdbDiscoverMoviesRequestDto filters)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var queryParams = new Dictionary<string, string>
        {
            ["page"] = filters.Page.ToString(),
            ["language"] = filters.Language,
            ["include_adult"] = filters.IncludeAdult.ToString().ToLowerInvariant(),
            ["include_video"] = filters.IncludeVideo.ToString().ToLowerInvariant()
        };

        void AddStringParam(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                queryParams[key] = value;
            }
        }

        void AddIntParam(string key, int? value)
        {
            if (value.HasValue)
            {
                queryParams[key] = value.Value.ToString();
            }
        }

        void AddDoubleParam(string key, double? value)
        {
            if (value.HasValue)
            {
                queryParams[key] = value.Value.ToString(
                    CultureInfo.InvariantCulture);
            }
        }

        AddStringParam("sort_by", filters.SortBy);
        AddStringParam("with_genres", filters.WithGenres);
        AddStringParam("without_genres", filters.WithoutGenres);
        AddIntParam("primary_release_year", filters.PrimaryReleaseYear);

        AddStringParam("primary_release_date.gte",
            filters.ReleaseDateGte?.ToString("yyyy-MM-dd"));

        AddStringParam("primary_release_date.lte",
            filters.ReleaseDateLte?.ToString("yyyy-MM-dd"));

        AddDoubleParam("vote_average.gte", filters.VoteAverageGte);
        AddDoubleParam("vote_average.lte", filters.VoteAverageLte);
        AddIntParam("vote_count.gte", filters.VoteCountGte);
        AddIntParam("vote_count.lte", filters.VoteCountLte);
        AddIntParam("with_runtime.gte", filters.WithRuntimeGte);
        AddIntParam("with_runtime.lte", filters.WithRuntimeLte);

        AddStringParam("with_original_language", filters.WithOriginalLanguage);
        AddStringParam("region", filters.Region);
        
        var queryString = string.Join("&", queryParams.Select(param =>
            $"{Uri.EscapeDataString(param.Key)}=" +
            $"{Uri.EscapeDataString(param.Value)}"));

        var response = await client.GetAsync(
            $"discover/movie?{queryString}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TmdbMovieResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
    
    public async Task<TmdbMovieDetailsDto?> GetMovieDetailsAsync(int movieId)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"movie/{movieId}?language=uk-UA");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TmdbMovieDetailsDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
    
    public async Task<TmdbMovieResponseDto?> GetPopularMoviesAsync(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"movie/popular?language=uk-UA&page={page}");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<TmdbMovieResponseDto>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}