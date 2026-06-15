namespace WatchDiary.Models.Dtos;

public class CreateCollectionDto
{
    public int UserId { get; set; }
    public string CollectionName { get; set; } = null!;
}
