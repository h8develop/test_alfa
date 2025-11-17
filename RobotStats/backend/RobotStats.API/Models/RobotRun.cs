using System.ComponentModel.DataAnnotations;

namespace RobotStats.Models
{
    public class RobotRun
    {
        public int Id { get; set; }
        
        [Required]
        public string RobotName { get; set; } = string.Empty;
        
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        
        public DateTime? EndTime { get; set; }
        
        [Required]
        public string Status { get; set; } = "Running";
        
        public int TimeSavedMinutes { get; set; }
        
        public string? ErrorMessage { get; set; }
    }
}