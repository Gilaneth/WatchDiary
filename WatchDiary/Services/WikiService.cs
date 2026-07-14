using System.Text.Json;

namespace WatchDiary.Services;

public class WikiService
{
    private readonly HttpClient _http;
    private const string BaseUrl = "https://en.wikipedia.org/w/api.php";

    public WikiService(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.Add("User-Agent", "WatchDiaryApp/1.0");
    }

    public async Task<string?> GetPlotAsync(string movieName)
    {
        var url = $"{BaseUrl}?action=query&prop=extracts&explaintext=true&titles={Uri.EscapeDataString(movieName)}&format=json";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(json);
        var pages = document.RootElement.GetProperty("query").GetProperty("pages");

        foreach (var page in pages.EnumerateObject())
        {
            if (page.Value.TryGetProperty("extract", out var extractElement))
            {
                var fullText = extractElement.GetString();
                if (string.IsNullOrWhiteSpace(fullText)) return null;

                var cleanPlot = ExtractPlotOnly(fullText);
                return cleanPlot;
            }
        }

        return null;
    }

    private string? ExtractPlotOnly(string fullText)
    {
        var plotHeaders = new[] { "== Plot ==\n", "== Synopsis ==\n", "== Premise ==\n", "== Plot summary ==\n" };
        int startIndex = -1;
        string matchedHeader = "";

        foreach (var header in plotHeaders)
        {
            startIndex = fullText.IndexOf(header, StringComparison.OrdinalIgnoreCase);
            if (startIndex != -1)
            {
                matchedHeader = header;
                break;
            }
        }

        if (startIndex == -1)
        {
            return fullText.Length > 4000 ? fullText.Substring(0, 4000).Trim() : fullText.Trim();
        }

        int plotStart = startIndex + matchedHeader.Length;

        int nextSectionIndex = fullText.IndexOf("\n==", plotStart);

        string plotText;
        if (nextSectionIndex != -1)
        {
            plotText = fullText.Substring(plotStart, nextSectionIndex - plotStart);
        }
        else
        {
            plotText = fullText.Substring(plotStart);
        }

        plotText = plotText.Trim();

        return plotText.Length > 8000 ? plotText.Substring(0, 8000).Trim() : plotText;
    }
}