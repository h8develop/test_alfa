namespace RobotStats.Models
{
    public class DashboardMetrics
    {
        public int TotalRuns { get; set; }
        public int SuccessfulRuns { get; set; }
        public int FailedRuns { get; set; }
        public int RunningRuns { get; set; }
        public int TotalTimeSavedMinutes { get; set; }
        public double AverageTimeSavedMinutes { get; set; }
        public List<RobotRun> RecentRuns { get; set; } = new();
    }
}