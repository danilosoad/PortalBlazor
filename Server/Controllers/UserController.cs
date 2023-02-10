using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalBlazor.Domain.Entity;
using PortalBlazor.Infra.Data;

namespace PortalBlazor.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}