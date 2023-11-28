using Interface;
using Interface.User;
using System.Text.RegularExpressions;

namespace Logic.User
{
    public class UserRegistration
    {
        private IUserRegistrationDAL _userDAL;

        public UserRegistration(IUserRegistrationDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public bool SignUp(string email, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrEmpty(email))
                errors.Add("email is required");

            if (errors.Count == 0)
            {
                string emailRegEx = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                + "@"
                + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
                if (!Regex.IsMatch(email, emailRegEx))
                    errors.Add("email must be valid");
                if (_userDAL.CheckIfUserExists(email) != 0)
                    errors.Add("user already exists");
            }

            if (errors.Count != 0) // if any errors occurred
            {
                return false;
            }
            else
            {
                UserDTO userDTO = new UserDTO();
                userDTO.Email = email.ToLower();
                string key = Functions.GetUniqueKey(60);
                userDTO.Password = BCrypt.Net.BCrypt.HashPassword(key);
                userDTO.Role = UserRoles.Passenger;

                if (_userDAL.InsertUser(userDTO) == 1)
                {
                    Functions.WriteToTxt(email.ToLower(), key); //email the password
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Register(UserModel userModel, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrEmpty(userModel.Password))
                errors.Add("password is required");

            if (string.IsNullOrEmpty(userModel.FirstName))
                errors.Add("first name is required");

            if (string.IsNullOrEmpty(userModel.LastName))
                errors.Add("last name is required");

            if (errors.Count == 0)
            {
                if (userModel.Password.Length < 7)
                    errors.Add("password must be at least 8 characters");
                if (!Regex.IsMatch(userModel.Password, @"[a-z]"))
                    errors.Add("password must contain at least 1 lowercase letter");
                if (!Regex.IsMatch(userModel.Password, @"[A-Z]"))
                    errors.Add("password must contain at least 1 uppercase letter");
                if (!Regex.IsMatch(userModel.Password, @"[\d]"))
                    errors.Add("password must contain at least 1 digit");
                if (!Regex.IsMatch(userModel.Password, @"[!-\/:-@[-`{-~]"))
                    errors.Add("password must contain at least 1 special character");
            }


            if (errors.Count == 0)
            {
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                userModel.RegisterUser(_userDAL);
                return true;
            } else
            {
                return false;
            }
        }

        public UserModel? Login(string email, string password, out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrEmpty(email))
                errors.Add("email is required");
            if (string.IsNullOrEmpty(password))
                errors.Add("password is required");

            if (errors.Count == 0) // if all required fields are properly filled
            {
                UserModel user = null;
                UserDTO userDTO = _userDAL.FindByEmail(email.ToLower());
                if (userDTO is not null)
                {
                    user = new UserModel(userDTO);
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                        return user;
                    else //incorrect password
                        errors.Add("login failed");
                }
                else
                { //incorrect username
                    errors.Add("login failed");
                }
            }

            return null;
        }
    }
}