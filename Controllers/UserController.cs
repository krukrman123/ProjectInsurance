using ProjectInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectInsurance.Models;
using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Controllers
{
    [Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _db;
	

		public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
		{
			_userManager = userManager;
			_db = db;
			
		}
		//Listing Policy Holders linked to user
		public async Task<IActionResult> Index(string? id, int pg = 1)
		{
			if (id == "")
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}

			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			UserViewModel userViewModel = new UserViewModel();

			UserViewModel thisModel = new UserViewModel();
			thisModel.UserId = user.Id;
			thisModel.Name = user.Name;
			thisModel.LastName = user.LastName;
			thisModel.StreetName = user.StreetName;
			thisModel.BuildingNumber = user.BuildingNumber;
			thisModel.CityName = user.CityName;
			thisModel.ZipCode = user.ZipCode;
			thisModel.Email = user.Email;
			thisModel.TelephoneNumber = user.TelephoneNumber;
			thisModel.PolicyHolders = await GetPolicyHolders(id);

			//prevents from accessing a the page by any user, redirecting to "error" page
			if (thisModel.PolicyHolders.Count > 0)
			{
				if (thisModel.PolicyHolders.First().UserId != _userManager.GetUserId(User))
				{
					return RedirectToAction("NotFoundCustom", "Home");
				}
			}

			//pagination
			const int pageSize = 5;
			if (pg < 1)
			{
				pg = 1;
			}
			int recsCount = thisModel.PolicyHolders.Count();
			var pager = new PagerModel(recsCount, pg, pageSize);
			int recSkip = (pg - 1) * pageSize;

			thisModel.PolicyHolders = thisModel.PolicyHolders.Skip(recSkip).Take(pager.PageSize).ToList();
			userViewModel = thisModel;

			this.ViewBag.Pager = pager;

			return View(userViewModel);
		}
		private async Task<List<PolicyHolderModel>> GetPolicyHolders(string? id)
		{
			var policyHolders = from i in _db.Insured
								where i.UserId == id
								select i;
			return policyHolders.ToList();
		}

		
	}
}
