using Interface.Railway;

namespace TestData.Railway
{
    public class RailwayDALTest : IRailwayContainerDAL, IRailwayDAL
    {
        public List<RailwayDTO> railways = new List<RailwayDTO>(); //public for assert test

        public RailwayDALTest()
        {
            railways.Add(new RailwayDTO() { RailwayId = 1, StartStationId = 1, EndStationId = 2, State = true, Length = 20 });
            railways.Add(new RailwayDTO() { RailwayId = 2, StartStationId = 2, EndStationId = 3, State = true, Length = 12 });
            railways.Add(new RailwayDTO() { RailwayId = 3, StartStationId = 3, EndStationId = 5, State = true, Length = 7 });
        }

        public RailwayDTO FindById(int id)
        {
            return railways.Find(x => x.RailwayId == id);
        }

        public List<RailwayDTO> GetRailways()
        {
            return railways;
        }

        public int InsertRailway(RailwayDTO railwayDTO)
        {
            railwayDTO.RailwayId = railways.Count + 1;

            railways.Add(railwayDTO);
            return 1;
        }

        public int UpdateRailway(RailwayDTO railwayDTO)
        {
            try
            {
                railways[railways.FindIndex(x => x.RailwayId == railwayDTO.RailwayId)] = railwayDTO;
            } catch 
            {
                return 0;
            }
            return 1;
        }

        public int DeleteRailway(int id)
        {
            try
            {
                railways.RemoveAt(railways.FindIndex(x => x.RailwayId == id));
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        public int DisableLinkedJourneys(int railwayId)
        {
            throw new NotImplementedException();
        }

        public RailwayDTO FindByStations(int startStationId, int endStationId)
        {
            throw new NotImplementedException();
        }
    }
}