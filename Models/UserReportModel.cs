using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Models
{
    public class UserReportModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public List<InsuranceModel>? Insurances { get; set; }
        public List<PolicyHolderModel>? PolicyHolders { get; set; }
        public List<InsuranceEventModel>? InsuranceEvents { get; set; }
    }
}
