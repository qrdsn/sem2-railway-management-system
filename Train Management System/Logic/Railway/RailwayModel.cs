using Interface;
using Interface.Railway;

namespace Logic.Railway
{
    public class RailwayModel
    {
        public int RailwayId;
        public int StartStationId;
        public string StartStationName;
        public int EndStationId;
        public string EndStationName;
        public bool State;
        public int Length;

        public RailwayModel(RailwayDTO railwayDTO)
        {
            RailwayId = railwayDTO.RailwayId;
            StartStationId = railwayDTO.StartStationId;
            StartStationName = railwayDTO.StartStationName;
            EndStationId = railwayDTO.EndStationId;
            EndStationName = railwayDTO.EndStationName;
            State = railwayDTO.State;
            Length = railwayDTO.Length;
        }

        public RailwayModel(int startStationId, int endStationId, bool state, int length)
        {
            StartStationId = startStationId;
            EndStationId = endStationId;
            State = state;
            Length = length;
        }

        public bool UpdateRailway(IRailwayDAL railwayDAL)
        {
            RailwayDTO railwayDTO = new RailwayDTO() { RailwayId = RailwayId, StartStationId = StartStationId, EndStationId = EndStationId, State = State, Length = Length };

            if (railwayDAL.UpdateRailway(railwayDTO) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public RailwayModel() { } //for something i forgot y
    }
}