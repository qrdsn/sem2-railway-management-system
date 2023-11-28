using System.Data.SqlClient;

namespace Data.F_Train
{
    public class TrainDAL : ITrainContainerDAL, ITrainDAL
    {
        string ConnectionString = "Data Source=mssqlstud.fhict.local;Initial Catalog=dbi489971;User ID=dbi489971;Password=thisismypassword";

        public TrainDTO FindById(int id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM [train] WHERE id = @id", con))
                {
                    cmd.Parameters.Add(new SqlParameter("id", id));

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    rdr.Read();

                    return new TrainDTO() { train_id = (int)rdr["id"], type = (TrainTypes)rdr["type"], max_speed = (int)rdr["max_speed"] };
                }
            }
        }

        public List<TrainDTO> GetTrains()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM [station]", con))
                {
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    List<TrainDTO> output = new List<TrainDTO>();

                    while (rdr.Read())
                    {
                        output.Add(new TrainDTO() { train_id = (int)rdr["id"], type = (TrainTypes)rdr["type"], max_speed = (int)rdr["max_speed"] });
                    }

                    return output;
                }
            }
        }

        public int InsertTrain(TrainDTO train)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [train] (type, max_speed) VALUES (@type, @max_speed)", con))
                {
                    con.Open();

                    cmd.Parameters.Add(new SqlParameter("@type", train.type));
                    cmd.Parameters.Add(new SqlParameter("@max_speed", train.max_speed));

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int UpdateTrain(TrainDTO train)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE [train] set type = @type, max_speed = @max_speed WHERE id = @id", con))
                {
                    con.Open();

                    cmd.Parameters.Add(new SqlParameter("@type", train.type));
                    cmd.Parameters.Add(new SqlParameter("@max_speed", train.max_speed));
                    cmd.Parameters.Add(new SqlParameter("@id", train.train_id));

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int DeleteTrain(int train_id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"DELETE [train] WHERE id = @id", con))
                {
                    con.Open();

                    cmd.Parameters.Add(new SqlParameter("@id", train_id));

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
