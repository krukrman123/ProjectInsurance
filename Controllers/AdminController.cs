using ProjectInsurance.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectInsurance.Areas.Identity.Data;

namespace ProjectInsurance.Controllers
{
    [Authorize(Roles = "Administrator")]
	public class AdminController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _db;




		public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_db = db;
          


        }

        
        //Listing Users, their Insured and their linked Insurances and Events
        public async Task<IActionResult> Index()
		{
			//getting all Users from db
			//var users = await _userManager.Users.ToListAsync();
			var users = from user in _db.Users
						orderby user.LastName
						select user;

		

			//getting insured from db
			var insuredFromDb = _db.Insured.ToList();
			//getting insurances from db
			var insurancesFromDb = _db.Insurance.ToList();
			//getting events from db
			var eventsFromDb = _db.Event.ToList();

			AdminUserModel adminUserModel = new AdminUserModel();

			AdminUserModel thisModel = new AdminUserModel();
			thisModel.Users = users.ToList();
			thisModel.PolicyHolders = insuredFromDb.ToList();
			thisModel.Insurances = insurancesFromDb.ToList();
			thisModel.InsuranceEvents = eventsFromDb.ToList();
			adminUserModel = thisModel;

			return View(adminUserModel);
		}
		

		

		//PolicyHolder/Create/id
		//GET
		public IActionResult CreatePolicyHolder(string? userId)
		{
			if (userId == "")
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			

			PolicyHolderModel policyHolder = new PolicyHolderModel();
			string userHolderId = (string)userId;

			policyHolder.UserId = userHolderId;
			return View(policyHolder);
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreatePolicyHolder(PolicyHolderModel obj)
		{
			if (ModelState.IsValid)
			{
				obj.Name = (obj.Name.Trim().ToLower());
				obj.LastName = (obj.LastName.Trim().ToLower());
				obj.EMail = obj.EMail.Trim();
				obj.TelephoneNumber = obj.TelephoneNumber.Trim();
				obj.StreetName = (obj.StreetName.Trim());
				obj.BuildingNumber = obj.BuildingNumber.Trim();
				obj.CityName = (obj.CityName.Trim());
				obj.ZIPCode = obj.ZIPCode.Trim();

				_db.Insured.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojištěnec uložen.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//PolicyHolder/Edit/id
		//GET
		public IActionResult EditPolicyHolder(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuredFromDb = _db.Insured.Find(id);

			if (insuredFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			

			return View(insuredFromDb);
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditPolicyHolder(PolicyHolderModel obj)
		{
			if (ModelState.IsValid)
			{
				var events = from j in _db.Event
							 where j.PolicyHolderId == obj.Id
							 select j;

				foreach (var e in events)
				{
					e.PolicyHolderName = obj.Name;
					e.PolicyHolderLastName = obj.LastName;
					_db.Event.Update(e);
				}

				_db.Insured.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojištěnec aktualizován.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//PolicyHolder/Delete/id
		//GET
		public IActionResult DeletePolicyHolder(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuredFromDb = _db.Insured.Find(id);

			if (insuredFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
		

			return View(insuredFromDb);
		}
		//POST
		[HttpPost, ActionName("DeletePolicyHolder")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOSTPolicyHolder(int? id)
		{
			var obj = _db.Insured.Find(id);
			if (obj == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			_db.Insured.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Pojištěnec odstraněn.";
			return RedirectToAction("Index");
		}

	

	

		//Insurance/Create/id
		//GET
		public IActionResult CreateInsurance(int? userId)
		{
			if (userId == null || userId == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}

			var insuredFromDb = _db.Insured.Find(userId);

			if (insuredFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
		

			var dateTime = DateTime.Now.Date;
			InsuranceModel insurance = new InsuranceModel();
			int holderId = (int)userId;
			insurance.InsuranceValidFrom = dateTime;
			insurance.InsuranceValidUntil = dateTime;
			insurance.InsuranceHolderId = holderId;

			ViewBag.Name = insuredFromDb.Name;
			ViewBag.LastName = insuredFromDb.LastName;
			return View(insurance);
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateInsurance(InsuranceModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Insurance.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojištění uloženo.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//Insurance/Edit/id
		//GET
		public IActionResult EditInsurance(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceFromDb = _db.Insurance.Find(id);

			if (insuranceFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}

		

			return View(insuranceFromDb);
		}
		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditInsurance(InsuranceModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Insurance.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojištění aktualizováno.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//Insurance/Delete/id
		//GET
		public IActionResult DeleteInsurance(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceFromDb = _db.Insurance.Find(id);

			if (insuranceFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
		

			int insuranceId = insuranceFromDb.InsuranceHolderId;
			var insuredFromDb = _db.Insured.Find(insuranceId);
			

			ViewBag.Name = insuredFromDb.Name;
			ViewBag.LastName = insuredFromDb.LastName;
			return View(insuranceFromDb);
		}
		//POST
		[HttpPost, ActionName("DeleteInsurance")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOSTInsurance(int? id)
		{
			var obj = _db.Insurance.Find(id);
			if (obj == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			_db.Insurance.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Pojištění odstraněno.";
			return RedirectToAction("Index");

		}
		


		//InsuranceEvent/Create/id
		//GET
		public IActionResult CreateInsuranceEvent(int? insuranceId)
		{
			if (insuranceId == null || insuranceId == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceFromDb = _db.Insurance.Find(insuranceId);
			if (insuranceFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			int holderId = insuranceFromDb.InsuranceHolderId;
			var insuredFromDb = _db.Insured.Find(holderId);
			if (insuredFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
		

			var dateTime = DateTime.Now.Date;
			InsuranceEventModel insuranceEvent = new InsuranceEventModel();
			insuranceEvent.InsuranceEventTime = dateTime;
			insuranceEvent.InsuranceId = insuranceFromDb.Id;
			insuranceEvent.PolicyHolderId = insuredFromDb.Id;
			insuranceEvent.PolicyHolderName = insuredFromDb.Name;
			insuranceEvent.PolicyHolderLastName = insuredFromDb.LastName;
			insuranceEvent.InsuranceType = insuranceFromDb.InsuranceType;
			insuranceEvent.InsuranceSubject = insuranceFromDb.InsuranceSubject;

			return View(insuranceEvent);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CreateInsuranceEvent(InsuranceEventModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Event.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojistná událost uložena.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//InsuranceEvent/Edit/id
		//GET
		public IActionResult EditInsuranceEvent(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceEventFromDb = _db.Event.Find(id);

			if (insuranceEventFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			

			return View(insuranceEventFromDb);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditInsuranceEvent(InsuranceEventModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Event.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Pojistná událost aktualizována.";
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		//InsuranceEvent/Delete/id
		//GET
		public IActionResult DeleteInsuranceEvent(int? id)
		{
			if (id == null || id == 0)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var insuranceEventFromDb = _db.Event.Find(id);

			if (insuranceEventFromDb == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			

			return View(insuranceEventFromDb);
		}

		//POST
		[HttpPost, ActionName("DeleteInsuranceEvent")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOSTInsuranceEvent(int? id)
		{
			var obj = _db.Event.Find(id);
			if (obj == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			_db.Event.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "Pojistná událost odstraněna.";
			return RedirectToAction("Index");
		}



		public async Task<IActionResult> IndexRole()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}

		//Getting users and their roles
		public async Task<IActionResult> IndexUserRole()
		{
			var users = await _userManager.Users.ToListAsync();
			var userRolesViewModel = new List<UserRolesViewModel>();
			foreach (ApplicationUser user in users)
			{
				var thisViewModel = new UserRolesViewModel();
				thisViewModel.UserId = user.Id;
				thisViewModel.Name = user.Name;
				thisViewModel.LastName = user.LastName;
				thisViewModel.Email = user.Email;
				thisViewModel.TelephoneNumber = user.TelephoneNumber;
				thisViewModel.Roles = await GetUserRoles(user);
				userRolesViewModel.Add(thisViewModel);
			}
			return View(userRolesViewModel);
		}
		private async Task<List<string>> GetUserRoles(ApplicationUser user)
		{
			return new List<string>(await _userManager.GetRolesAsync(user));
		}

		public IActionResult CreateRole()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateRole(RolesModel role)
		{
			var roleExist = await _roleManager.RoleExistsAsync(role.RoleName);
			if (!roleExist)
			{
				var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName.Trim()));
			}
			TempData["success"] = "Role úspěšně vytvořena";
			return View();
		}

		//GET
		public async Task<IActionResult> DeleteRole(string? id)
		{
			if (id == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			var role = await _roleManager.FindByIdAsync(id);

			if (role == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			return View(role);
		}
		//POST
		[HttpPost, ActionName("DeleteRole")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteRolePOST(string? id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			await _roleManager.DeleteAsync(role);
			TempData["success"] = "Role úspěšně odstraněna";
			return RedirectToAction("IndexRole");

		}


		//GET
		public async Task<IActionResult> Manage(string userId)
		{
			ViewBag.userId = userId;
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return RedirectToAction("NotFoundCustom", "Home");
			}
			ViewBag.UserName = user.UserName;
			var manageUserRolesViewModel = new List<ManageUserRolesViewModel>();
			foreach (var role in _roleManager.Roles)
			{
				var userRolesViewModel = new ManageUserRolesViewModel
				{
					RoleId = role.Id,
					RoleName = role.Name
				};
				if (await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRolesViewModel.Selected = true;
				}
				else
				{
					userRolesViewModel.Selected = false;
				}
				manageUserRolesViewModel.Add(userRolesViewModel);
			}
			return View(manageUserRolesViewModel);
		}

		//Adding/removing roles to users
		//POST
		[HttpPost]
		public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return View();
			}
			var roles = await _userManager.GetRolesAsync(user);
			var result = await _userManager.RemoveFromRolesAsync(user, roles);
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cannot remove user existing roles");
				return View(model);
			}
			result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Cannot add selected roles to user");
				return View(model);
			}
			return RedirectToAction("IndexUserRole");
		}


	}
}
