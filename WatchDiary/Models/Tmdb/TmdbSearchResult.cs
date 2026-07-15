namespace WatchDiary.Models.Tmdb;

public class TmdbSearchResponse
{
    public List<TmdbMovie> Results { get; set; } = [];
}

public class TmdbMovie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public string? Overview { get; set; }
    public string? Release_Date { get; set; }
    public string? First_Air_Date { get; set; }
    public string? Poster_Path { get; set; }
    public double Vote_Average { get; set; }
    public string? Media_Type { get; set; }
    public List<int> Genre_Ids { get; set; } = [];
}

public class TmdbCredits
{
    public List<TmdbCastMember> Cast { get; set; } = [];
}

public class TmdbCastMember
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Character { get; set; }
    public int Order { get; set; }
}
