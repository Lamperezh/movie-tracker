using System.Globalization;
using movie_tracker.DTOs.Tmdb.Movie;
using movie_tracker.DTOs.Tmdb.TV;

namespace movie_tracker.Services;

public static class TmdbQueryBuilder
{
    public static Dictionary<string, string> BuildMovieQuery(TmdbDiscoverMoviesRequestDto filters)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["page"] = filters.Page.ToString(),
            ["language"] = filters.Language,
            ["include_adult"] = filters.IncludeAdult.ToString().ToLowerInvariant(),
            ["include_video"] = filters.IncludeVideo.ToString().ToLowerInvariant()
        };

        AddStringParam(queryParams, "sort_by", filters.SortBy);
        AddStringParam(queryParams, "with_genres", filters.WithGenres);
        AddStringParam(queryParams, "without_genres", filters.WithoutGenres);

        AddIntParam(queryParams, "primary_release_year", filters.PrimaryReleaseYear);

        AddStringParam(queryParams, "primary_release_date.gte",
            filters.ReleaseDateGte?.ToString("yyyy-MM-dd"));

        AddStringParam(queryParams, "primary_release_date.lte",
            filters.ReleaseDateLte?.ToString("yyyy-MM-dd"));

        AddDoubleParam(queryParams, "vote_average.gte", filters.VoteAverageGte);
        AddDoubleParam(queryParams, "vote_average.lte", filters.VoteAverageLte);

        AddIntParam(queryParams, "vote_count.gte", filters.VoteCountGte);
        AddIntParam(queryParams, "vote_count.lte", filters.VoteCountLte);

        AddIntParam(queryParams, "with_runtime.gte", filters.WithRuntimeGte);
        AddIntParam(queryParams, "with_runtime.lte", filters.WithRuntimeLte);

        AddStringParam(queryParams, "with_original_language",
            filters.WithOriginalLanguage);

        AddStringParam(queryParams, "region", filters.Region);

        return queryParams;
    }

    public static Dictionary<string, string> BuildTvQuery(TmdbDiscoverTvRequestDto filters)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["page"] = filters.Page.ToString(),
            ["language"] = filters.Language,
            ["include_adult"] = filters.IncludeAdult.ToString().ToLowerInvariant(),
            ["include_null_first_air_dates"] =
                filters.IncludeNullFirstAirDates.ToString().ToLowerInvariant()
        };

        AddStringParam(queryParams, "sort_by", filters.SortBy);
        AddStringParam(queryParams, "with_genres", filters.WithGenres);
        AddStringParam(queryParams, "without_genres", filters.WithoutGenres);

        AddIntParam(queryParams, "first_air_date_year", filters.FirstAirDateYear);

        AddStringParam(queryParams, "first_air_date.gte",
            filters.FirstAirDateGte?.ToString("yyyy-MM-dd"));

        AddStringParam(queryParams, "first_air_date.lte",
            filters.FirstAirDateLte?.ToString("yyyy-MM-dd"));

        AddStringParam(queryParams, "air_date.gte",
            filters.AirDateGte?.ToString("yyyy-MM-dd"));

        AddStringParam(queryParams, "air_date.lte",
            filters.AirDateLte?.ToString("yyyy-MM-dd"));

        AddDoubleParam(queryParams, "vote_average.gte", filters.VoteAverageGte);
        AddDoubleParam(queryParams, "vote_average.lte", filters.VoteAverageLte);

        AddIntParam(queryParams, "vote_count.gte", filters.VoteCountGte);
        AddIntParam(queryParams, "vote_count.lte", filters.VoteCountLte);

        AddIntParam(queryParams, "with_runtime.gte", filters.WithRuntimeGte);
        AddIntParam(queryParams, "with_runtime.lte", filters.WithRuntimeLte);

        AddStringParam(queryParams, "with_original_language",
            filters.WithOriginalLanguage);

        AddStringParam(queryParams, "with_networks", filters.WithNetworks);
        AddStringParam(queryParams, "with_origin_country", filters.WithOriginCountry);
        AddStringParam(queryParams, "with_status", filters.WithStatus);
        AddStringParam(queryParams, "with_type", filters.WithType);
        AddStringParam(queryParams, "timezone", filters.Timezone);

        if (filters.ScreenedTheatrically.HasValue)
        {
            queryParams["screened_theatrically"] =
                filters.ScreenedTheatrically.Value.ToString().ToLowerInvariant();
        }

        return queryParams;
    }

    private static void AddStringParam(Dictionary<string, string> queryParams, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            queryParams[key] = value;
        }
    }

    private static void AddIntParam(Dictionary<string, string> queryParams, string key, int? value)
    {
        if (value.HasValue)
        {
            queryParams[key] = value.Value.ToString();
        }
    }

    private static void AddDoubleParam(Dictionary<string, string> queryParams, string key, double? value)
    {
        if (value.HasValue)
        {
            queryParams[key] = value.Value.ToString(
                CultureInfo.InvariantCulture);
        }
    }
}