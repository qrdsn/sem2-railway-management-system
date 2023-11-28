namespace ASP.Models
{
    public class JourneyViewModel
    {
        public int JourneyId { get; set; }
        public int RailwayId { get; set; }
        public int TrainId { get; set; }
        public int StartStationId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public bool State { get; set; }
        public TimeSpan? AdjustedDepartureTime { get; set; }
        public TimeSpan? AdjustedArrivalTime { get; set; }
    }
}
