using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchDiary.Data;

namespace WatchDiary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly WatchDiaryDbContext _context;
        public UsersController(WatchDiaryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }
    }
}