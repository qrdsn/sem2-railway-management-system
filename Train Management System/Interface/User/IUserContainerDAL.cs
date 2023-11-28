using Interface.Station;

namespace Interface.User
{
    public interface IUserContainerDAL
    {
        public List<UserDTO> GetUsers(UserRoles role);
        public List<UserDTO> GetAllUsers();
        public UserDTO FindById(int id);
        public int InsertUser(UserDTO userDTO);
        public int DeleteUser(int id);
        public int CheckIfUserExists(string email);
        public UserDTO FindByEmail(string email);
        public int DeleteLinkedTickets(int userId);
    }
}