using Interface.Railway;

namespace Logic.Railway
{
    public class RailwayContainer
    {
        private IRailwayContainerDAL _railwayDAL;

        public RailwayContainer(IRailwayContainerDAL railwayDAL)
        {
            _railwayDAL = railwayDAL;
        }

        public List<RailwayModel> GetAllRailways()
        {
            List<RailwayDTO> railwayDTOList = _railwayDAL.GetRailways();
            if (railwayDTOList == null)
                return null;

            List<RailwayModel> railwayList = new List<RailwayModel>();

            foreach (RailwayDTO railwayDTO in railwayDTOList)
            {
                railwayList.Add(new RailwayModel(railwayDTO));
            }

            return railwayList;
        }

        public RailwayModel FindRailwayById(int id)
        {
            if (id == 0)
                return null;
            RailwayDTO railwayDTO = _railwayDAL.FindById(id);
            if (railwayDTO == null)
                return null;
            else
                return new RailwayModel(railwayDTO);
        }

        public RailwayModel FindRailwayStations(int startStationId, int endStationId)
        {
            return new RailwayModel(_railwayDAL.FindByStations(startStationId, endStationId));
        }

        public bool InsertRailway(RailwayModel railway)
        {
            RailwayDTO railwayDTO = new RailwayDTO() { StartStationId = railway.StartStationId, EndStationId = railway.EndStationId, State = railway.State, Length = railway.Length };

            if (_railwayDAL.InsertRailway(railwayDTO) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteRailway(int railway_id)
        {
            if (_railwayDAL.DeleteRailway(railway_id) == 1 && _railwayDAL.DisableLinkedJourneys(railway_id) >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}