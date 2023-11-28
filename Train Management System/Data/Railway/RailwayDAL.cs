using Interface;
using Interface.Railway;
using System.Data.SqlClient;

namespace Data.Railway
{
    public class RailwayDAL : IRailwayContainerDAL, IRailwayDAL
    {
        public RailwayDTO FindById(int id)
        {
            string q = @"SELECT railway.id, railway.state, railway.length, railway.start_station, starting_station.name as start_station_name, railway.end_station, ending_station.name as end_station_name
                            FROM [railway] 
                            JOIN [station] as starting_station
                            ON railway.start_station = starting_station.id
                            JOIN [station] as ending_station
                            ON railway.end_station = ending_station.id
                            WHERE railway.id = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, RailwayDTO> mapping = rdr =>
            {
                return new RailwayDTO() { RailwayId = (int)rdr["id"], StartStationId = (int)rdr["start_station"], StartStationName = (string)rdr["start_station_name"], EndStationId = (int)rdr["end_station"], EndStationName = (string)rdr["end_station_name"], State = (bool)rdr["state"], Length = (int)rdr["length"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        public List<RailwayDTO> GetRailways()
        {
            // what data to get
            string q = @"SELECT railway.id, railway.state, railway.length, railway.start_station, starting_station.name as start_station_name, railway.end_station, ending_station.name as end_station_name
                            FROM [railway] 
                            JOIN [station] as starting_station
                            ON railway.start_station = starting_station.id
                            JOIN [station] as ending_station
                            ON railway.end_station = ending_station.id;";

            var parms = new Dictionary<string, object> {};

            // how to format the data for usage
            Func<SqlDataReader, List<RailwayDTO>> mapping = rdr =>
            {
                List<RailwayDTO> output = new List<RailwayDTO>();

                while (rdr.Read())
                {
                    output.Add(new RailwayDTO() { RailwayId = (int)rdr["id"], StartStationId = (int)rdr["start_station"], StartStationName = (string)rdr["start_station_name"], EndStationId = (int)rdr["end_station"], EndStationName = (string)rdr["end_station_name"], State = (bool)rdr["state"], Length = (int)rdr["length"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public int InsertRailway(RailwayDTO railwayDTO)
        {
            string q = @"INSERT INTO [railway] (start_station, end_station, state, length) 
                            VALUES (@Sstation, @Estation, @state, @length)";

            var parms = new Dictionary<string, object>
            {
                {"@Sstation", railwayDTO.StartStationId},
                {"@Estation", railwayDTO.EndStationId},
                {"@state", railwayDTO.State},
                {"@length", railwayDTO.Length}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int UpdateRailway(RailwayDTO railwayDTO)
        {
            string q = @"UPDATE [railway] set start_station = @Sstation, end_station = @Estation, state = @state, length = @length WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@Sstation", railwayDTO.StartStationId},
                {"@Estation", railwayDTO.EndStationId},
                {"@state", railwayDTO.State},
                {"@length", railwayDTO.Length},
                {"@id", railwayDTO.RailwayId}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteRailway(int id)
        {
            string q = @"DELETE [railway] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// disables journeys that use given railwayid
        /// </summary>
        /// <param name="railwayId"></param>
        /// <returns></returns>
        public int DisableLinkedJourneys(int railwayId)
        {
            string q = @"UPDATE [journey] SET state = 0 WHERE railway_id = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", railwayId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public RailwayDTO FindByStations(int startStationId, int endStationId)
        {
            string q = @"SELECT railway.id, railway.state, railway.length, railway.start_station, starting_station.name as start_station_name, railway.end_station, ending_station.name as end_station_name
                            FROM [railway] 
                            JOIN [station] as starting_station
                            ON railway.start_station = starting_station.id
                            JOIN [station] as ending_station
                            ON railway.end_station = ending_station.id
                            WHERE railway.start_station = @ssId AND railway.end_station = @esId;";

            var parms = new Dictionary<string, object>
            {
                {"@ssId", startStationId},
                {"@esId", endStationId},
            };

            // how to format the data for usage
            Func<SqlDataReader, RailwayDTO> mapping = rdr =>
            {
                return new RailwayDTO() { RailwayId = (int)rdr["id"], StartStationId = (int)rdr["start_station"], StartStationName = (string)rdr["start_station_name"], EndStationId = (int)rdr["end_station"], EndStationName = (string)rdr["end_station_name"], State = (bool)rdr["state"], Length = (int)rdr["length"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }
    }
}
