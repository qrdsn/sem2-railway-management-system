using Interface.Station;

namespace Logic.Station
{
    public class StationModel
    {
        public int StationId { get; set; } //private set?
        public string Location { get; set; } //private set?
        public string Name { get; set; } //private set?

        public StationModel(StationDTO stationDTO)
        {
            StationId = stationDTO.StationId;
            Location = stationDTO.Location;
            Name = stationDTO.Name;
        }

        public StationModel(string location, string name)
        {
            Location = location;
            Name = name;
        }

        public bool UpdateStation(IStationDAL stationDAL)
        {
            StationDTO stationDTO = new StationDTO() { StationId = StationId, Name = Name, Location = Location };

            if (stationDAL.UpdateStation(stationDTO) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}