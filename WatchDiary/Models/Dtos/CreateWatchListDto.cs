namespace WatchDiary.Models.Dtos;

public class CreateWatchListDto
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public WatchStatus Status { get; set; }
}
