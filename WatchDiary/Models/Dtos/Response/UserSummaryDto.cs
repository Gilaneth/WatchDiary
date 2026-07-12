namespace WatchDiary.Models.Dtos.Response;

public class UserSummaryDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}
