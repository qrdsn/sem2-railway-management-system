using Interface.Railway;
using Interface.Station;
using Logic.Railway;

namespace Logic.Station
{
    public class StationContainer
    {
        private IStationContainerDAL _stationDAL;

        public StationContainer(IStationContainerDAL stationDAL)
        {
            _stationDAL = stationDAL;
        }

        public List<StationModel> GetAllStations()
        {
            List<StationDTO> stationDTOlist = _stationDAL.GetStations();

            if (stationDTOlist == null)
                return null;

            List<StationModel> stationList = new List<StationModel>();
            
            
            foreach (StationDTO stationDTO in stationDTOlist)
            {
                stationList.Add(new StationModel(stationDTO));
            }

            return stationList;
        }

        public StationModel FindStationById(int id)
        {
            StationDTO stationDTO = _stationDAL.FindById(id);
            if (stationDTO == null)
                return null;
            else 
                return new StationModel(_stationDAL.FindById(id));
        }

        public bool InsertStation(StationModel station)
        {
            StationDTO stationDTO = new StationDTO() {Name = station.Name, Location = station.Location};

            if (_stationDAL.InsertStation(stationDTO) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool DeleteStation(int stationId)
        {
            if (_stationDAL.DeleteStation(stationId) == 1 && _stationDAL.DisableLinkedRailways(stationId) >= 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// returns a dictionary <stationId, railways connected to said station>   to be used in find fastest route method
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, List<RailwayModel>> GetConnectingRailways()
        {
            List<StationDTO> stationDTOList = _stationDAL.GetStations();

            Dictionary<int, List<RailwayModel>> stationList = new Dictionary<int, List<RailwayModel>>();

            foreach(StationDTO stationDTO in stationDTOList)
            {
                if (stationDTO == null)
                    continue;
                List<RailwayDTO> railwayDTOList =_stationDAL.GetLinkedRailways(stationDTO.StationId);
                if (railwayDTOList == null)
                    continue;
                List<RailwayModel> railwayModelList = new List<RailwayModel>();
                foreach (RailwayDTO railwayDTO in railwayDTOList)
                {
                    if (railwayDTO == null)
                        continue;
                    railwayModelList.Add(new RailwayModel(railwayDTO));
                }

                stationList.Add(stationDTO.StationId, railwayModelList);
            }

            return stationList;
        }
    }
}