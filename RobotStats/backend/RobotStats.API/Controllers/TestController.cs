using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotStats.Data;

namespace RobotStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("db")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new { 
                    message = "Database connection successful", 
                    canConnect = canConnect 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    message = "Database connection failed", 
                    error = ex.Message 
                });
            }
        }
    }
}