using Data;
using System.Data.SqlClient;

namespace Interface
{
    public static class DatabaseContext
    {
        private static readonly string _ConnectionString = "Data Source=mssqlstud.fhict.local;Initial Catalog=dbi489971;User ID=dbi489971;Password=thisismypassword";

        public static int ExecuteNonQuery(string queryString, Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // create parameters
                    foreach (KeyValuePair<string, object> parm in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(parm.Key, parm.Value ?? (object)DBNull.Value)); //insert/update dbnull if parm object is null
                    }

                    try
                    {
                        con.Open();

                        return cmd.ExecuteNonQuery();
                    } catch
                    {
                        throw new DatabaseConnectionFailedException();
                    }
                }
            }
        }

        public static T GetSingle<T>(string queryString, Dictionary<string, object> parameters, Func<SqlDataReader, T> mappingFunction)
            where T : class
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // create parameters
                    foreach (KeyValuePair<string, object> parm in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(parm.Key, parm.Value));
                    }

                    try
                    {
                        con.Open();

                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.HasRows)
                        {
                            rdr.Read();

                            return mappingFunction.Invoke(rdr);
                        } else
                        {
                            return null;
                        }
                    }
                    catch
                    {
                        throw new DatabaseConnectionFailedException();
                    }
                }
            }
        }

        public static List<T> GetList<T>(string queryString, Dictionary<string, object> parameters, Func<SqlDataReader, List<T>> mappingFunction)
            where T : class
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    // create parameters
                    foreach (KeyValuePair<string, object> parm in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(parm.Key, parm.Value));
                    }

                    try
                    {
                        con.Open();

                        SqlDataReader rdr = cmd.ExecuteReader();

                        if (rdr.HasRows)
                        {
                            return mappingFunction.Invoke(rdr);
                        } else
                        {
                            return null;
                        }
                    }
                    catch
                    {
                        throw new DatabaseConnectionFailedException();
                    }
                }
            }
        }

        public static object ExecuteScalar(string queryString, Dictionary<string, object> parameters)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryString, con))
                {
                    foreach (KeyValuePair<string, object> parm in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(parm.Key, parm.Value));
                    }

                    try
                    {
                        con.Open();

                        object result = cmd.ExecuteScalar();

                        return result;
                    }
                    catch
                    {
                        throw new DatabaseConnectionFailedException();
                    }

                }
            }
        }
    }
}
