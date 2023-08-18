using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Models
{
    public class AdminUserModel
    {
        public List<InsuranceModel>? Insurances { get; set; }
        public List<InsuranceEventModel>? InsuranceEvents { get; set; }
        public List<PolicyHolderModel>? PolicyHolders { get; set; }
        public List<ApplicationUser>? Users { get; set; }
    }
}
