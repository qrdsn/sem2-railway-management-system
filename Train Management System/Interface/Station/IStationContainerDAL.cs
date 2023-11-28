using Interface.Railway;

namespace Interface.Station
{
    public interface IStationContainerDAL
    {
        public List<StationDTO> GetStations();
        public StationDTO FindById(int id);
        public int InsertStation(StationDTO stationDTO);
        public int DeleteStation(int id);
        public int DisableLinkedRailways(int stationId);
        public List<RailwayDTO> GetLinkedRailways(int stationId);
    }
}