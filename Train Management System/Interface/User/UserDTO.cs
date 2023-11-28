namespace Interface.User
{
    public class UserDTO
    {
        public int UserId;
        public string Email;
        public string Password;
        public string? FirstName;
        public string? LastName;
        public UserRoles Role;
        public int? JourneyId;
        public EmployeePositions? EmployeePosition;
    }
}
