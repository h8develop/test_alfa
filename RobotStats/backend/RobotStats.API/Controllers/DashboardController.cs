using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobotStats.Data;
using RobotStats.Models;

namespace RobotStats.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardMetrics>> GetMetrics()
        {
            var runs = await _context.RobotRuns.ToListAsync();
            
            var metrics = new DashboardMetrics
            {
                TotalRuns = runs.Count,
                SuccessfulRuns = runs.Count(r => r.Status == "Success"),
                FailedRuns = runs.Count(r => r.Status == "Failed"),
                RunningRuns = runs.Count(r => r.Status == "Running"),
                TotalTimeSavedMinutes = runs.Where(r => r.Status == "Success").Sum(r => r.TimeSavedMinutes),
                RecentRuns = runs.OrderByDescending(r => r.StartTime).Take(10).ToList()
            };

            metrics.AverageTimeSavedMinutes = metrics.SuccessfulRuns > 0 
                ? Math.Round((double)metrics.TotalTimeSavedMinutes / metrics.SuccessfulRuns, 2)
                : 0;

            return metrics;
        }
    }
}