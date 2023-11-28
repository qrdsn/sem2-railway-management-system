namespace Interface.Railway
{
    public interface IRailwayContainerDAL
    {
        public List<RailwayDTO> GetRailways();
        public RailwayDTO FindById(int id);
        public RailwayDTO FindByStations(int startStationId, int endStationId);
        public int DeleteRailway(int id);
        public int InsertRailway(RailwayDTO railwayDTO);
        public int DisableLinkedJourneys(int railwayId);
    }
}