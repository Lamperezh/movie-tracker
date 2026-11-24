using System.Net.Http.Json;

namespace Persistence.Services;

public static class TmdbHelper
{
    public static async Task<T?> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static string BuildQueryString(Dictionary<string, string> queryParams)
    {
        return string.Join("&", queryParams.Select(param =>
            $"{Uri.EscapeDataString(param.Key)}=" +
            $"{Uri.EscapeDataString(param.Value)}"));
    }
}