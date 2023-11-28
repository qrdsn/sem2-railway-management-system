using Interface.Railway;
using Interface.Station;

namespace TestData.Station
{
    public class StationDALTest : IStationContainerDAL, IStationDAL
    {
        public List<StationDTO> stations = new List<StationDTO>(); //public for assert test

        public StationDALTest()
        {
            stations.Add(new StationDTO() { StationId = 1, Name = "tilburg central", Location = "tilburg" });
            stations.Add(new StationDTO() { StationId = 2, Name = "tilburg university", Location = "tilburg" });
            stations.Add(new StationDTO() { StationId = 3, Name = "reeshof", Location = "tilburg" });
        }

        public StationDTO FindById(int id)
        {
            return stations.Find(x => x.StationId == id);
            //actual find
        }

        public List<StationDTO> GetStations()
        {
            return stations;
        }

        public int InsertStation(StationDTO stationDTO)
        {
            stationDTO.StationId = stations.Count + 1;

            stations.Add(stationDTO);
            return 1;
        }

        public int UpdateStation(StationDTO stationDTO)
        {
            try
            {
                stations[stations.FindIndex(x => x.StationId == stationDTO.StationId)] = stationDTO;
            } catch 
            {
                return 0;
            }
            return 1;
        }

        public int DeleteStation(int id)
        {
            try
            {
                stations.RemoveAt(stations.FindIndex(x => x.StationId == id));
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        public int DisableLinkedRailways(int stationId)
        {
            throw new NotImplementedException();
        }

        public List<RailwayDTO> GetLinkedRailways(int stationId)
        {
            throw new NotImplementedException();
        }
    }
}