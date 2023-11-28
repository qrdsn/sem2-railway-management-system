namespace Interface.Journey
{
    public class JourneyDTO
    {
        public int JourneyId;
        public int RailwayId;
        public int TrainId;
        public int StartStationId;
        public TimeSpan DepartureTime;
        public TimeSpan ArrivalTime;
        public bool State;
        public TimeSpan? AdjustedDepartureTime;
        public TimeSpan? AdjustedArrivalTime;
    }
}