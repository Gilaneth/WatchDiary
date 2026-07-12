namespace WatchDiary.Models.Dtos;

public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
}
