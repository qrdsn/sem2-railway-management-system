using Interface.Journey;

namespace Logic.Journey
{
    public class JourneyModel
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

        public JourneyModel(JourneyDTO journeyDTO)
        {
            JourneyId = journeyDTO.JourneyId;
            RailwayId = journeyDTO.RailwayId;
            TrainId = journeyDTO.TrainId;
            StartStationId = journeyDTO.StartStationId;
            DepartureTime = journeyDTO.DepartureTime;
            ArrivalTime = journeyDTO.ArrivalTime;
            State = journeyDTO.State;
            AdjustedDepartureTime = journeyDTO.AdjustedDepartureTime;
            AdjustedArrivalTime = journeyDTO.AdjustedArrivalTime;
        }

        public JourneyModel(int railwayId, int trainId, int startStationId, TimeSpan departureTime, TimeSpan arrivalTime, bool state)
        {
            RailwayId = railwayId;
            TrainId = trainId;
            StartStationId = startStationId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            State = state;
        }

        public int UpdateJourney(IJourneyDAL journeyDAL)
        {
            JourneyDTO journeyDTO = new JourneyDTO() {JourneyId = JourneyId, RailwayId = RailwayId, TrainId = TrainId, StartStationId = StartStationId, DepartureTime = DepartureTime, ArrivalTime = ArrivalTime, State = State, AdjustedDepartureTime = AdjustedDepartureTime, AdjustedArrivalTime = AdjustedArrivalTime };

            return journeyDAL.UpdateJourney(journeyDTO);
        }
    }
}