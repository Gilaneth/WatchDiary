using Microsoft.EntityFrameworkCore;
using WatchDiary.Models;

namespace WatchDiary.Data
{

    public class WatchDiaryDbContext : DbContext
    {
        public WatchDiaryDbContext(DbContextOptions<WatchDiaryDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }


    }
}
