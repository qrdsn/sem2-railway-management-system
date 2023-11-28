using Interface;
using Interface.User;
using System.Text;
using System.Text.RegularExpressions;

namespace Logic.User
{
    public class UserContainer
    {
        private IUserContainerDAL _userDAL;

        public UserContainer(IUserContainerDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserDTO> userDTOList;

            userDTOList = _userDAL.GetAllUsers();

            if (userDTOList == null)
                return null;

            List<UserModel> userList = new List<UserModel>();

            foreach (UserDTO userDTO in userDTOList)
            {
                userList.Add(new UserModel(userDTO));
            }

            return userList;
        }

        public List<UserModel> GetAllUsers(UserRoles role)
        {
            List<UserDTO> userDTOList;

            userDTOList = _userDAL.GetUsers(role);

            List<UserModel> userList = new List<UserModel>();
            
            foreach (UserDTO userDTO in userDTOList)
            {
                userList.Add(new UserModel(userDTO));
            }

            return userList;
        }

        public UserModel FindUserById(int id)
        {
            return new UserModel(_userDAL.FindById(id));
        }

        public bool CreateUser(UserModel user, out List<string> errors)
        {
            errors = new List<string>();

            string emailRegEx = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                   + "@"
                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            if (string.IsNullOrEmpty(user.Email))
            {
                errors.Add("email is required");
            }
            else if (!Regex.IsMatch(user.Email, emailRegEx))
            {
                errors.Add("email must be valid");
            }
            else if (_userDAL.CheckIfUserExists(user.Email) != 0)
            {
                errors.Add("user already exists");
            }

            if (user.Role == UserRoles.Employee && user.EmployeePosition is null)
                errors.Add(UserRoles.Employee.ToString() + " position is required");

            if (errors.Count == 0)
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Email = user.Email.ToLower();
                string key = Functions.GetUniqueKey(60);
                userDTO.Password = BCrypt.Net.BCrypt.HashPassword(key);
                Functions.WriteToTxt(user.Email.ToLower(), key); //email the password

                if (string.IsNullOrEmpty(user.FirstName))
                    user.FirstName = null;
                userDTO.FirstName = user.FirstName;
                if (string.IsNullOrEmpty(user.LastName))
                    user.LastName = null;
                userDTO.LastName = user.LastName;
                userDTO.Role = user.Role;
                userDTO.EmployeePosition = user.EmployeePosition;

                int affectedRows = _userDAL.InsertUser(userDTO);
                if (affectedRows == 1 || (affectedRows == 2 && user.Role == UserRoles.Employee))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DeleteUser(int userId)
        {
            int affectedRows = _userDAL.DeleteUser(userId);
            int disabledTickets =  _userDAL.DeleteLinkedTickets(userId);
            if (affectedRows == 1 && disabledTickets >= 0 || affectedRows == 2 && disabledTickets >= 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}