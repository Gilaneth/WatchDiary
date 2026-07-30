using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class WatchDiaryDbContext : DbContext
    {
        public WatchDiaryDbContext(DbContextOptions<WatchDiaryDbContext> options) : base(options) { }

    }
}
