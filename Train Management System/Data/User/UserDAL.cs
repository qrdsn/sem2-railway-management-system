using Interface;
using Interface.Station;
using Interface.User;
using System.Data.SqlClient;

namespace Data.User
{
    public class UserDAL : IUserContainerDAL, IUserRegistrationDAL, IUserDAL
    {
        public UserDTO FindById(int id)
        {
            string q = @"SELECT * FROM [user]
                        LEFT JOIN [employee] ON [user].id = employee.user_id
                        where [user].id = @id";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            // how to format the data for usage
            Func<SqlDataReader, UserDTO> mapping = rdr =>
            {
                return new UserDTO() { UserId = (int)rdr["id"], Email = (string)rdr["email"], FirstName = DBNull.Value.Equals(rdr["first_name"]) ? null : (string)rdr["first_name"], LastName = DBNull.Value.Equals(rdr["last_name"]) ? null : (string)rdr["last_name"], Role = (UserRoles)rdr["role"], JourneyId = DBNull.Value.Equals(rdr["journey_id"]) ? null : (int)rdr["journey_id"], EmployeePosition = DBNull.Value.Equals(rdr["position"]) ? null : (EmployeePositions)rdr["position"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        /// <summary>
        /// gets users with specifici role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<UserDTO> GetUsers(UserRoles role)
        {
            // what data to get
            string q = @"SELECT [user].id, [user].email, [user].first_name, [user].last_name, [user].role, [employee].journey_id, [employee].position FROM [user]
                        LEFT JOIN [employee] ON [user].id = employee.user_id
                        where [user].role = @role";

            var parms = new Dictionary<string, object> 
            {
                {"@role", role}
            };

            // how to format the data for usage
            Func<SqlDataReader, List<UserDTO>> mapping = rdr =>
            {
                List<UserDTO> output = new List<UserDTO>();

                while (rdr.Read())
                {
                    output.Add(new UserDTO() { UserId = (int)rdr["id"], Email = (string)rdr["email"], FirstName = DBNull.Value.Equals(rdr["first_name"]) ? null : (string)rdr["first_name"], LastName = DBNull.Value.Equals(rdr["last_name"]) ? null : (string)rdr["last_name"], Role = (UserRoles)rdr["role"], JourneyId = DBNull.Value.Equals(rdr["journey_id"]) ? null : (int)rdr["journey_id"], EmployeePosition = DBNull.Value.Equals(rdr["position"]) ? null : (EmployeePositions)rdr["position"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public List<UserDTO> GetAllUsers()
        {
            // what data to get
            string q = @"SELECT [user].id, [user].email, [user].first_name, [user].last_name, [user].role, [employee].journey_id, [employee].position FROM [user]
                        LEFT JOIN [employee] ON [user].id = employee.user_id";

            var parms = new Dictionary<string, object>
            {
            };

            // how to format the data for usage
            Func<SqlDataReader, List<UserDTO>> mapping = rdr =>
            {
                List<UserDTO> output = new List<UserDTO>();

                while (rdr.Read())
                {
                    output.Add(new UserDTO() { UserId = (int)rdr["id"], Email = (string)rdr["email"], FirstName = DBNull.Value.Equals(rdr["first_name"]) ? null : (string)rdr["first_name"], LastName = DBNull.Value.Equals(rdr["last_name"]) ? null : (string)rdr["last_name"], Role = (UserRoles)rdr["role"], JourneyId = DBNull.Value.Equals(rdr["journey_id"]) ? null : (int)rdr["journey_id"], EmployeePosition = DBNull.Value.Equals(rdr["position"]) ? null : (EmployeePositions)rdr["position"] });
                }

                return output;
            };

            // executing the retrieval 
            return DatabaseContext.GetList(q, parms, mapping);
        }

        public int InsertUser(UserDTO userDTO)
        {
            string q = @"BEGIN TRANSACTION 
                        DECLARE @outputID int;
                        INSERT INTO [user] (email, password, first_name, last_name, role)
                        VALUES (@email, @password, @fname, @lname, @role);
                        SELECT @outputID = scope_identity();
                        IF @role = 1
	                        BEGIN INSERT INTO [employee] (user_id, journey_id, position) VALUES (@outputID, @journeyId, @position)
                        END;
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@email", userDTO.Email},
                {"@password", userDTO.Password},
                {"@fname", userDTO.FirstName},
                {"@lname", userDTO.LastName},
                {"@role", userDTO.Role},
                {"@journeyId", userDTO.JourneyId},
                {"@position", userDTO.EmployeePosition}
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int UpdateUser(UserDTO userDTO)
        {
            string q = @"BEGIN TRANSACTION
                        UPDATE [user] set email = @email, first_name = @fname, last_name = @lname, role = @role WHERE id = @uid
                        IF @role = @employee
	                        BEGIN  
		                        IF EXISTS (SELECT user_id FROM [employee] where user_id = @uid)
			                        UPDATE [employee] set journey_id = @jid, position = @position WHERE user_id = @uid;
		                        ELSE
			                        INSERT INTO [employee] (user_id, journey_id, position) VALUES (@uid, @jid, @position);
	                        END;
                        ELSE
	                        DELETE [employee] WHERE user_id = @uid;
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@employee", UserRoles.Employee},
                {"@uid", userDTO.UserId},
                {"@email", userDTO.Email},
                {"@fname", userDTO.FirstName},
                {"@lname", userDTO.LastName},
                {"@role", userDTO.Role},
                {"@jid", userDTO.JourneyId != 0 ? userDTO.JourneyId : DBNull.Value},
                {"@position", userDTO.EmployeePosition},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// updates user with email, not id
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public int UpdateUserWithEmail(UserDTO userDTO)
        {
            string q = @"UPDATE [user] set password = @password, first_name = @fname, last_name = @lname WHERE email = @email";

            var parms = new Dictionary<string, object>
            {
                {"@email", userDTO.Email},
                {"@password", userDTO.Password},
                {"@fname", userDTO.FirstName},
                {"@lname", userDTO.LastName},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        public int DeleteUser(int id)
        {
            string q = @"BEGIN TRANSACTION 
                        DECLARE @outputID int;
                        SELECT @outputID = role FROM [user] WHERE id = @id;
                        IF @outputID = 1
	                        DELETE [employee] WHERE user_id = @id;
                        DELETE [user] WHERE id = @id;
                        COMMIT;";

            var parms = new Dictionary<string, object>
            {
                {"@id", id},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }

        /// <summary>
        /// returns the id from user with given email, or returns 0 if user doesn't exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public int CheckIfUserExists(string email)
        {
            string q = @"SELECT id FROM [user] WHERE email = @email";

            var parms = new Dictionary<string, object>
            {
                {"@email", email},
            };

            return Convert.ToInt32(DatabaseContext.ExecuteScalar(q, parms));
        }

        /// <summary>
        /// returns user with given email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserDTO FindByEmail(string email)
        {
            string q = @"SELECT [user].id, [user].email, [user].password, [user].first_name, [user].last_name, [user].role, [employee].journey_id, [employee].position FROM [user]
                        LEFT JOIN [employee] ON [user].id = employee.user_id
                        where [user].email = @email";

            var parms = new Dictionary<string, object>
            {
                {"@email", email},
            };

            // how to format the data for usage
            Func<SqlDataReader, UserDTO> mapping = rdr =>
            {
                return new UserDTO() { UserId = (int)rdr["id"], Email = (string)rdr["email"], Password = (string)rdr["password"], FirstName = DBNull.Value.Equals(rdr["first_name"]) ? null : (string)rdr["first_name"], LastName = DBNull.Value.Equals(rdr["last_name"]) ? null : (string)rdr["last_name"], Role = (UserRoles)rdr["role"], JourneyId = DBNull.Value.Equals(rdr["journey_id"]) ? null : (int)rdr["journey_id"], EmployeePosition = DBNull.Value.Equals(rdr["position"]) ? null : (EmployeePositions)rdr["position"] };
            };

            // executing the retrieval 
            return DatabaseContext.GetSingle(q, parms, mapping);
        }

        /// <summary>
        /// disables tickets with given userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteLinkedTickets(int userId)
        {
            string q = @"BEGIN TRANSACTION 
                        DECLARE @outputID int;
                        SELECT @outputID = id FROM [ticket] WHERE user_id = @id;
                        DELETE FROM [ticket] WHERE user_id = @id;
                        DELETE FROM [journey_ticket] WHERE ticket_id = @outputID;
                        COMMIT";

            var parms = new Dictionary<string, object>
            {
                {"@id", userId},
            };

            return DatabaseContext.ExecuteNonQuery(q, parms);
        }
    }
}