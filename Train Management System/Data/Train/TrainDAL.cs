using Interface;
using Interface.Train;
using System.Data.SqlClient;

namespace Data.Train
{
    public class TrainDAL : ITrainContainerDAL, ITrainDAL
    {
        public TrainDTO FindById(int id)
        {
            string q = @"SELECT * FROM [train] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, TrainDTO> mapping = rdr =>
            {
                return new TrainDTO() { TrainId = (int)rdr["id"], Type = (TrainTypes)rdr["type"], MaxSpeed = (int)rdr["max_speed"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        public List<TrainDTO> GetTrains()
        {
            // what data to get
            string q = @"SELECT * FROM [train]";

            var parms = new Dictionary<string, object> {};

            // how to format the data for usage
            Func<SqlDataReader, List<TrainDTO>> mapping = rdr =>
            {
                List<TrainDTO> output = new List<TrainDTO>();

                while (rdr.Read())
                {
                    output.Add(new TrainDTO() { TrainId = (int)rdr["id"], Type = (TrainTypes)rdr["type"], MaxSpeed = (int)rdr["max_speed"], FirstClassSeats = getSeatAmount((int)rdr["id"], 1), SecondClassSeats = getSeatAmount((int)rdr["id"], 2) });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        /// <summary>
        /// gets amount of seats in train from given type
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int getSeatAmount(int trainId, int type){
            string q = @"SELECT COUNT(*) FROM [seat] WHERE train_id = @trainId AND type = @type";

            var parms = new Dictionary<string, object>
            {
                {"@trainId", trainId },
                {"@type", type}
            };

            return (int)DatabaseContext.ExecuteScalar(q, parms);
        }

        /// <summary>
        /// inserts train, and insert amount of seats from each type
        /// </summary>
        /// <param name="trainDTO"></param>
        /// <returns></returns>
        public int InsertTrain(TrainDTO trainDTO)
        {
            string q = @"BEGIN TRANSACTION 
                                    DECLARE @outputID int;
                                    INSERT INTO [train] (type, max_speed) 
                                    VALUES (@type, @max_speed) 
                                    SELECT @outputID = scope_identity();
                                    INSERT INTO [seat] (train_id, type, location_name) VALUES ";

            string positions = "ABCD";
            int posLength = positions.Length;
            int pos = 0;
            int i = 1;
            for (i = i; i < trainDTO.FirstClassSeats + 1; i++)
            {
                string position = (positions[pos].ToString() + Math.Ceiling((decimal)i / (decimal)posLength));
                if (i == trainDTO.FirstClassSeats && trainDTO.SecondClassSeats == 0)
                    q = q + @$"(@outputID, 1, '{position}')";
                else
                    q = q + @$"(@outputID, 1, '{position}'), ";


                pos++;
                if (pos == posLength)
                    pos = 0;
            }

            int ii = i;
            for (i = i; i < trainDTO.SecondClassSeats + ii; i++)
            {
                string position = (positions[pos].ToString() + Math.Ceiling((decimal)i / (decimal)posLength));
                if (i == trainDTO.SecondClassSeats + ii - 1)
                    q = q + @$"(@outputID, 2, '{position}')";
                else
                    q = q + @$"(@outputID, 2, '{position}'), ";

                pos++;
                if (pos == posLength)
                    pos = 0;
            }

            q = q + " COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@type", (int)trainDTO.Type},
                {"@max_speed", trainDTO.MaxSpeed}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int UpdateTrain(TrainDTO trainDTO)
        {
            string q = @"UPDATE [train] set type = @type, max_speed = @max_speed WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@type", trainDTO.Type},
                {"@max_speed", trainDTO.MaxSpeed },
                {"@id", trainDTO.TrainId }
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteTrain(int id)
        {
            string q = @"DELETE FROM [train] WHERE id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// disables journeys that are linked to given train
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public int DisableLinkedJourneys(int trainId)
        {
            string q = @"UPDATE [journey] SET state = 0 WHERE train_id = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", trainId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// disconnects all users from given journey
        /// </summary>
        /// <param name="trainId"></param>
        /// <returns></returns>
        public int UpdateLinkedEmployees(int trainId)
        {
            string q = @"UPDATE [employee] SET journey_id = null WHERE journey_id = @id;";

            var parms = new Dictionary<string, object>
            {
                {"@id", trainId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteSeats(int trainId)
        {
            string q = @"DELETE FROM [seat] WHERE train_id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", trainId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }
    }
}
