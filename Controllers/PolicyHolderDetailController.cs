using ProjectInsurance.Areas.Identity.Data;
using ProjectInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProjectInsurance.Controllers
{
    [Authorize]
    public class PolicyHolderDetailController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public PolicyHolderDetailController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        //Policy holder detail with linked Insurances
        public async Task<IActionResult> PolicyHolderDetail(int? id, int pg = 1)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }
            
            var insuredFromDb = await _db.Insured.FindAsync(id);
            if (insuredFromDb == null)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }

            //prevents from accessing a PolicyHolder detail by any user, redirecting to "error" page
            if (insuredFromDb.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }

            PolicyHolderDetailModel policyHolderInsuranceModel = new PolicyHolderDetailModel();

            PolicyHolderDetailModel thisModel = new PolicyHolderDetailModel();
            thisModel.PolicyHolderId = insuredFromDb.Id;    
            thisModel.Name = insuredFromDb.Name;
            thisModel.LastName = insuredFromDb.LastName;
            thisModel.EMail = insuredFromDb.EMail;
            thisModel.TelephoneNumber = insuredFromDb.TelephoneNumber;  
            thisModel.StreetName = insuredFromDb.StreetName;
            thisModel.BuildingNumber = insuredFromDb.BuildingNumber;
            thisModel.CityName = insuredFromDb.CityName;
            thisModel.ZIPCode = insuredFromDb.ZIPCode;
            thisModel.Insurances = await GetInsurances(id);

            //pagination
            const int pageSize = 3;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = thisModel.Insurances.Count();
            var pager = new PagerModel(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;

            thisModel.Insurances = thisModel.Insurances.Skip(recSkip).Take(pager.PageSize).ToList();
            policyHolderInsuranceModel = thisModel;

            this.ViewBag.Pager = pager;

            return View(policyHolderInsuranceModel);

        }

        private async Task<List<InsuranceModel>> GetInsurances(int? id)
        {         
            var insuranecOfHolder = from i in _db.Insurance
                                    where i.InsuranceHolderId == id
                                    select i;
            return insuranecOfHolder.ToList();
        }
    }
}
