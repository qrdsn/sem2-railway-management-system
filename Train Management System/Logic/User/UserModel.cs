using Interface;
using Interface.User;

namespace Logic.User
{
    public class UserModel
    {
        public int UserId;
        public string Email;
        public string Password;
        public string? FirstName;
        public string? LastName;
        public UserRoles Role;
        public int? JourneyId;
        public EmployeePositions? EmployeePosition;

        public UserModel(UserDTO userDTO)
        {
            UserId = userDTO.UserId;
            Email = userDTO.Email;
            Password = userDTO.Password;
            FirstName = userDTO.FirstName;
            LastName = userDTO.LastName;
            Role = userDTO.Role;
            JourneyId = userDTO.JourneyId;
            EmployeePosition = userDTO.EmployeePosition;
        }

        public UserModel(string email, string firstName, string lastName, UserRoles role, EmployeePositions? employeePosition, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            EmployeePosition = employeePosition;
            Password = password;
        }

        public bool RegisterUser(IUserRegistrationDAL userDAL)
        {
            UserDTO userDTO = new UserDTO() { UserId = UserId, Email = Email, Password = Password, FirstName = FirstName, LastName = LastName };

            int affectedRows = userDAL.UpdateUserWithEmail(userDTO);

            if (affectedRows == 1)
                return true;
            else
                return false;
        }

        public bool UpdateUser(IUserDAL userDAL)
        {
            UserDTO userDTO = new UserDTO() { UserId = UserId, Email = Email, Password = Password, FirstName = FirstName, LastName = LastName, Role = Role, JourneyId = JourneyId, EmployeePosition = EmployeePosition};

            int affectedRows = userDAL.UpdateUser(userDTO);

            if (affectedRows == 1 || affectedRows == 2)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}