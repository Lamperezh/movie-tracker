using movie_tracker.DTOs.Tmdb.Actor;
using movie_tracker.DTOs.Tmdb.Movie;
using movie_tracker.DTOs.Tmdb.TV;

namespace movie_tracker.Services;

public class TmdbService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TmdbService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TmdbMovieResponseDto?> DiscoverMoviesAsync(TmdbDiscoverMoviesRequestDto filters)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var queryParams = TmdbQueryBuilder.BuildMovieQuery(filters);
        var queryString = TmdbHelper.BuildQueryString(queryParams);

        var response = await client.GetAsync(
            $"discover/movie?{queryString}");

        return await TmdbHelper.ReadResponseAsync<TmdbMovieResponseDto>(
            response);
    }

    public async Task<TmdbMovieDetailsDto?> GetMovieDetailsAsync(int movieId)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"movie/{movieId}?language=uk-UA&append_to_response=credits");

        return await TmdbHelper.ReadResponseAsync<TmdbMovieDetailsDto>(
            response);
    }

    public async Task<TmdbMovieResponseDto?> GetPopularMoviesAsync(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"movie/popular?language=uk-UA&page={page}");

        return await TmdbHelper.ReadResponseAsync<TmdbMovieResponseDto>(
            response);
    }

    public async Task<TmdbPersonDto?> GetPersonDetailsAsync(int personId)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"person/{personId}?language=uk-UA&append_to_response=movie_credits,tv_credits");

        return await TmdbHelper.ReadResponseAsync<TmdbPersonDto>(
            response);
    }

    public async Task<TmdbTvResponseDto?> DiscoverTvAsync(TmdbDiscoverTvRequestDto filters)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var queryParams = TmdbQueryBuilder.BuildTvQuery(filters);
        var queryString = TmdbHelper.BuildQueryString(queryParams);

        var response = await client.GetAsync(
            $"discover/tv?{queryString}");

        return await TmdbHelper.ReadResponseAsync<TmdbTvResponseDto>(
            response);
    }

    public async Task<TmdbTvDetailsDto?> GetTvDetailsAsync(int tvId)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"tv/{tvId}?language=uk-UA&append_to_response=credits");

        return await TmdbHelper.ReadResponseAsync<TmdbTvDetailsDto>(
            response);
    }

    public async Task<TmdbTvResponseDto?> GetPopularTvAsync(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("TMDB");

        var response = await client.GetAsync(
            $"tv/popular?language=uk-UA&page={page}");

        return await TmdbHelper.ReadResponseAsync<TmdbTvResponseDto>(
            response);
    }
}