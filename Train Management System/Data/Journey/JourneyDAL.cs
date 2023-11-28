using Interface;
using Interface.Journey;
using System.Data.SqlClient;

namespace Data.Journey
{
    public class JourneyDAL : IJourneyContainerDAL, IJourneyDAL
    {
        public JourneyDTO FindById(int id)
        {
            string q = @"SELECT * FROM [journey] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, JourneyDTO> mapping = rdr =>
            {
                return new JourneyDTO() { JourneyId = (int)rdr["id"], RailwayId = (int)rdr["railway_id"], TrainId = (int)rdr["train_id"], StartStationId = (int)rdr["start_station_id"], DepartureTime = (TimeSpan)rdr["departure_time"], ArrivalTime = (TimeSpan)rdr["arrival_time"], State = (bool)rdr["state"], AdjustedDepartureTime = DBNull.Value.Equals(rdr["adjusted_departure_time"]) ? null : (TimeSpan)rdr["adjusted_departure_time"], AdjustedArrivalTime = DBNull.Value.Equals(rdr["adjusted_arrival_time"]) ? null : (TimeSpan)rdr["adjusted_arrival_time"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        public List<JourneyDTO> GetJourneys()
        {
            // what data to get
            string q = @"SELECT * FROM [journey]";

            var parms = new Dictionary<string, object> {};

            // how to format the data for usage
            Func<SqlDataReader, List<JourneyDTO>> mapping = rdr =>
            {
                List<JourneyDTO> output = new List<JourneyDTO>();

                while (rdr.Read())
                {
                    output.Add(new JourneyDTO() { JourneyId = (int)rdr["id"], RailwayId = (int)rdr["railway_id"], TrainId = (int)rdr["train_id"], StartStationId = (int)rdr["start_station_id"], DepartureTime = (TimeSpan)rdr["departure_time"], ArrivalTime = (TimeSpan)rdr["arrival_time"], State = (bool)rdr["state"], AdjustedDepartureTime = DBNull.Value.Equals(rdr["adjusted_departure_time"]) ? null : (TimeSpan)rdr["adjusted_departure_time"], AdjustedArrivalTime = DBNull.Value.Equals(rdr["adjusted_arrival_time"]) ? null : (TimeSpan)rdr["adjusted_arrival_time"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public int InsertJourney(JourneyDTO journeyDTO)
        {
            string q = @"INSERT INTO [journey] (railway_id, train_id, start_station_id, departure_time, arrival_time, state) VALUES (@rId, @tId, @ssId, @depTime, @arrTime, @state)";

            var parms = new Dictionary<string, object>
            {
                {"@rId", journeyDTO.RailwayId},
                {"@tId", journeyDTO.TrainId},
                {"@ssID", journeyDTO.StartStationId},
                {"@depTime", journeyDTO.DepartureTime},
                {"@arrTime", journeyDTO.ArrivalTime},
                {"@state", journeyDTO.State},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int UpdateJourney(JourneyDTO journeyDTO)
        {
            string q = @"UPDATE [journey] set railway_id = @rId, train_id = @tId, start_station_id = @ssId, departure_time = @depTime, arrival_time = @arrTime, state = @state, adjusted_departure_time = @adjDepTime, adjusted_arrival_time = @adjArrTime WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@rId", journeyDTO.RailwayId},
                {"@tId", journeyDTO.TrainId},
                {"@ssID", journeyDTO.StartStationId},
                {"@depTime", journeyDTO.DepartureTime},
                {"@arrTime", journeyDTO.ArrivalTime},
                {"@state", journeyDTO.State},
                {"@adjDepTime", journeyDTO.AdjustedDepartureTime},
                {"@adjArrTime", journeyDTO.AdjustedArrivalTime},
                {"@id", journeyDTO.JourneyId}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteJourney(int id)
        {
            string q = @"DELETE [journey] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// disables tickets that are linked to a specific journey
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns></returns>
        public int DisableLinkedTickets(int journeyId)
        {
            string q = @"DELETE FROM [ticket] WHERE journey_id = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", journeyId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }
    }
}
