using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotStats.Data;

namespace RobotStats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Проверяем подключение к БД
                var canConnect = await _context.Database.CanConnectAsync();
                return Ok(new { 
                    status = "Healthy", 
                    database = canConnect ? "Connected" : "Disconnected",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    status = "Unhealthy", 
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}