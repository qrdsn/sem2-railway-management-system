namespace Interface.User
{
    public interface IUserRegistrationDAL
    {
        public int InsertUser(UserDTO userDTO);
        public int CheckIfUserExists(string email);
        public UserDTO FindByEmail(string email);
        public int UpdateUserWithEmail(UserDTO userDTO);
    }
}