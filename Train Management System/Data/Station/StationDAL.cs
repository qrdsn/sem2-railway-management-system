using Interface;
using Interface.Railway;
using Interface.Station;
using System.Data.SqlClient;

namespace Data.Station
{
    public class StationDAL : IStationContainerDAL, IStationDAL
    {
         public StationDTO FindById(int id)
        {
            string q = @"SELECT * FROM [station] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, StationDTO> mapping = rdr =>
            {
                return new StationDTO() { StationId = (int)rdr["id"], Location = (string)rdr["location"], Name = (string)rdr["name"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        public List<StationDTO> GetStations()
        {
            // what data to get
            string q = @"SELECT * FROM [station]";

            var parms = new Dictionary<string, object> {};

            // how to format the data for usage
            Func<SqlDataReader, List<StationDTO>> mapping = rdr =>
            {
                List<StationDTO> output = new List<StationDTO>();

                while (rdr.Read())
                {
                    output.Add(new StationDTO() { StationId = (int)rdr["id"], Location = (string)rdr["location"], Name = (string)rdr["name"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public int InsertStation(StationDTO stationDTO)
        {
            string q = @"INSERT INTO [station] (name, location) VALUES (@name, @location)";

            var parms = new Dictionary<string, object>
            {
                {"@name", stationDTO.Name},
                {"@location", stationDTO.Location}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int UpdateStation(StationDTO stationDTO)
        {
            string q = @"UPDATE [station] set name = @name, location = @location WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@name", stationDTO.Name},
                {"@location", stationDTO.Location},
                {"@id", stationDTO.StationId}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteStation(int id)
        {
            string q = @"DELETE [station] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// disables railways that end or start with stationid
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public int DisableLinkedRailways(int stationId)
        {
            string q = @"UPDATE [railway] SET state = 0 WHERE start_station = @id OR end_station = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", stationId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// gets a list of railways that use given stationId as either start or end station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<RailwayDTO> GetLinkedRailways(int stationId)
        {
            // what data to get
            string q = @"SELECT * FROM [railway] WHERE start_station = @stationId OR end_station = @stationId";

            var parms = new Dictionary<string, object> {
                {"@stationId",  stationId}
            };

            // how to format the data for usage
            Func<SqlDataReader, List<RailwayDTO>> mapping = rdr =>
            {
                List<RailwayDTO> output = new List<RailwayDTO>();

                while (rdr.Read())
                {
                    output.Add(new RailwayDTO() { StartStationId = (int)rdr["start_station"], EndStationId = (int)rdr["end_station"], State = (bool)rdr["state"], Length = (int)rdr["length"], RailwayId = (int)rdr["id"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }
    }
}
