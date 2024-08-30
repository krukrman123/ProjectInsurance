using ProjectInsurance.Areas.Identity.Data;
using ProjectInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace ProjectInsurance.Controllers
{
    [Authorize]
    public class InsuranceEventModelsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public InsuranceEventModelsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

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
            //getting insured from db based on logged in user id
            var insuredFromDb = from i in _db.Insured
                                where i.UserId == user.Id
                                select i;

            if (insuredFromDb.Count() > 0)
            {
                if (insuredFromDb.First().UserId != _userManager.GetUserId(User))
                {
                    return RedirectToAction("NotFoundCustom", "Home");
                }
            }
            //creating list of IDs of PolicyHolders/Insured
            List<int> Ids = new List<int>();
            foreach (var insured in insuredFromDb)
            {
                int holderId = insured.Id;
                Ids.Add(holderId);
            }
            //getting insurances from db based on list of insured IDs
            var insurancesFromDb = _db.Insurance.Where(insurance => Ids.Contains(insurance.InsuranceHolderId))
                     .Select(a => a).ToList();
            //creating list of IDs of Insurances
            List<int> InsuranceIds = new List<int>();
            foreach (var insurance in insurancesFromDb)
            {
                int insuranceId = insurance.Id;
                InsuranceIds.Add(insuranceId);
            }
            //getting insurance events from db based on list of insurances IDs
            var insuranceEventsFromDb = _db.Event.Where(ev => InsuranceIds.Contains(ev.InsuranceId))
                     .Select(a => a).ToList();

            var holdersInsurancesEventsModel = new HoldersInsurancesEventsViewModel();

            var thisModel = new HoldersInsurancesEventsViewModel();
            thisModel.PolicyHolders = insuredFromDb.ToList();
            thisModel.Insurances = insurancesFromDb.ToList();
            thisModel.InsuranceEvents = insuranceEventsFromDb.ToList();

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
            holdersInsurancesEventsModel = thisModel;

            this.ViewBag.Pager = pager;

            return View(holdersInsurancesEventsModel);
        }

        //InsuranceEvent/Create/id
        //GET
        public IActionResult Create(int? insuranceId)
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
            //prevents from creating any InsuranceEvent under any user, redirecting to "error" page
            if (insuredFromDb.UserId != _userManager.GetUserId(User))
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
        public IActionResult Create(InsuranceEventModel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Event.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Pojistná událost uložena.";
                return RedirectToAction("Index", new { id = _userManager.GetUserId(User) });
            }
            return View(obj);
        }
        //InsuranceEvent/Edit/id
        //GET
        public IActionResult Edit(int? id)
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
            int holderId = insuranceEventFromDb.PolicyHolderId;
            var insuredFromDb = _db.Insured.Find(holderId);
            if (insuredFromDb == null)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }
            if (insuredFromDb.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }

            return View(insuranceEventFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InsuranceEventModel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Event.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Pojistná událost aktualizována.";
                return RedirectToAction("Index", new { id = _userManager.GetUserId(User) });
            }
            return View(obj);
        }
        //InsuranceEvent/Delete/id
        //GET
        public IActionResult Delete(int? id)
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
            int holderId = insuranceEventFromDb.PolicyHolderId;
            var insuredFromDb = _db.Insured.Find(holderId);
            if (insuredFromDb == null)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }
            if (insuredFromDb.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }

            return View(insuranceEventFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Event.Find(id);
            if (obj == null)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }
            _db.Event.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Pojistná událost odstraněna.";
            return RedirectToAction("Index", new { id = _userManager.GetUserId(User) });
        }
    }
}
