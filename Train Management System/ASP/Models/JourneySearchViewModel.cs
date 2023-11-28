namespace ASP.Models
{
    public class JourneySearchViewModel
    {
        public string StartStationName { get; set; }
        public string EndStationName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal JourneyPrice { get; set; }
    }
}