using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Controllers
{
    [Authorize]
	public class InsuranceDetailController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		public InsuranceDetailController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
		{
			_db = db;
			_userManager = userManager;
		}
		//InsuranceDetail/id
		public async Task<IActionResult> InsuranceDetail(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceFromDb = await _db.Insurance.FindAsync(id);


			if (insuranceFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}

			//prevents from accessing any Insurance by any user, redirecting to "error" page
			int insuranceId = insuranceFromDb.InsuranceHolderId;
			var insuredFromDb = _db.Insured.Find(insuranceId);
			if (insuredFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			if (insuredFromDb.UserId != _userManager.GetUserId(User))
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}

			return View(insuranceFromDb);
		}
	}
}
