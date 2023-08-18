using System.ComponentModel;

namespace ProjectInsurance.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        [DisplayName("FirstName")]
        public string Name { get; set; }
        [DisplayName("LastName")]
        public string LastName { get; set; }
        [DisplayName("Street")]
        public string StreetName { get; set; }
        [DisplayName("Street Number")]
        public string BuildingNumber { get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        [DisplayName("ZIP")]
        public string ZipCode { get; set; }
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [DisplayName("Phone Number")]
        public string TelephoneNumber { get; set; }

        public List<PolicyHolderModel> PolicyHolders { get; set; }

    }
}
