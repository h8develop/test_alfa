using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotStats.Data;
using RobotStats.Models;

namespace RobotStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotStatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RobotStatsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RobotRun>>> GetRobotRuns()
        {
            return await _context.RobotRuns
                .OrderByDescending(r => r.StartTime)
                .Take(50)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<RobotRun>> PostRobotRun(RobotRun robotRun)
        {
            robotRun.StartTime = DateTime.UtcNow;
            
            _context.RobotRuns.Add(robotRun);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRobotRuns), new { id = robotRun.Id }, robotRun);
        }
    }
}