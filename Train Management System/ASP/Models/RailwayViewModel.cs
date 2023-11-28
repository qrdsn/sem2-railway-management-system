namespace ASP.Models
{
    public class RailwayViewModel
    {
        public int RailwayId { get; set; }
        public int StartStationId { get; set; }
        public string StartStationName { get; set; }
        public int EndStationId { get; set; }
        public string EndStationName { get; set; }
        public bool State { get; set; }
        public int Length { get; set; }
    }
}
