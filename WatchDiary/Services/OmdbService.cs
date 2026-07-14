using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace WatchDiary.Services;

public class OmdbService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OmdbService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["Omdb:ApiKey"] ?? throw new Exception("OMDb API key not configured.");
    }

    public async Task<(decimal? ImdbRating, int? RtRating)> GetRatingsAsync(string imdbId)
    {
        if (string.IsNullOrEmpty(imdbId)) return (null, null);

        try
        {
            var url = $"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}";
            var response = await _http.GetFromJsonAsync<OmdbResponse>(url);

            if (response == null) return (null, null);

            decimal? imdb = decimal.TryParse(response.ImdbRating, System.Globalization.CultureInfo.InvariantCulture, out var parsedImdb)
                ? parsedImdb
                : null;

            int? rt = null;

            if (response.Ratings != null)
            {
                var rtRatingStr = response.Ratings.FirstOrDefault(r => r.Source == "Rotten Tomatoes")?.Value;
                if (!string.IsNullOrEmpty(rtRatingStr) && int.TryParse(rtRatingStr.Replace("%", ""), out var parsedRt))
                {
                    rt = parsedRt;
                }
            }

            return (imdb, rt);
        }
        catch
        {
            return (null, null);
        }
    }
}

public class OmdbResponse
{
    [JsonPropertyName("imdbRating")]
    public string? ImdbRating { get; set; }

    [JsonPropertyName("Ratings")]
    public List<OmdbRatingDto>? Ratings { get; set; }
}

public class OmdbRatingDto
{
    [JsonPropertyName("Source")]
    public string? Source { get; set; }

    [JsonPropertyName("Value")]
    public string? Value { get; set; }
}