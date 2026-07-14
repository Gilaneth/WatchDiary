using System.Text.Json.Serialization;

namespace WatchDiary.Models.Dtos.Response;

public class TmdbMovieDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("overview")]
    public string? Overview { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("poster_path")]
    public string? PosterPath { get; set; }

    [JsonPropertyName("genres")]
    public List<TmdbGenreDto>? Genres { get; set; }

    [JsonPropertyName("credits")]
    public TmdbCreditsDto? Credits { get; set; }

    [JsonPropertyName("aggregate_credits")]
    public TmdbCreditsDto? AggregateCredits { get; set; }

    [JsonPropertyName("external_ids")]
    public TmdbExternalIdsDto? ExternalIds { get; set; }

    [JsonPropertyName("vote_average")]
    public double? VoteAverage { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("first_air_date")]
    public string? FirstAirDate { get; set; }
}

public class TmdbGenreDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class TmdbCreditsDto
{
    [JsonPropertyName("cast")]
    public List<TmdbCastDto>? Cast { get; set; }
}

public class TmdbCastDto
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class TmdbExternalIdsDto
{
    [JsonPropertyName("imdb_id")]
    public string? ImdbId { get; set; }
}