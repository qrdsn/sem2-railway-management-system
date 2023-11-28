using Interface.User;

namespace ASP.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserRoles Role { get; set; }
        public int? JourneyId { get; set; }
        public EmployeePositions? EmployeePosition { get; set; }
    }
}
