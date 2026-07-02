using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Host=localhost;Database=watchDiaryDB;Username=postgres;Password=1234";

builder.Services.AddDbContext<WatchDiaryDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();