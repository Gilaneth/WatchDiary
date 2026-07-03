using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;


var builder = WebApplication.CreateBuilder(args);

var connectionString = "Host=localhost;Database=watchDiaryDB;Username=postgres;Password=1234";

builder.Services.AddDbContext<WatchDiaryDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();