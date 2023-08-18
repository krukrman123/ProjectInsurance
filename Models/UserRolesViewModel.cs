namespace ProjectInsurance.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
