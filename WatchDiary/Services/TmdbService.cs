using System.Text.Json;
using WatchDiary.Models.Tmdb;

namespace WatchDiary.Services;

public class TmdbService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.themoviedb.org/3";
    private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

    public TmdbService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Tmdb:ApiKey"] ?? throw new Exception("TMDB API key not configured.");
    }

    public async Task<TmdbSearchResponse> SearchAsync(string query, string mediaType = "multi")
    {
        var url = $"{BaseUrl}/search/{mediaType}?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&language=en-US";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<TmdbSearchResponse>(json, options) ?? new TmdbSearchResponse();
    }

    public async Task<TmdbMovie?> GetByIdAsync(int tmdbId, string mediaType = "movie")
    {
        var url = $"{BaseUrl}/{mediaType}/{tmdbId}?api_key={_apiKey}&language=en-US";
        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<TmdbMovie>(json, options);
    }

    public string GetImageUrl(string? posterPath) =>
        posterPath is null ? "" : $"{ImageBaseUrl}{posterPath}";
}
