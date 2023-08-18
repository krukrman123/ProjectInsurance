using ProjectInsurance.Areas.Identity.Data;
using ProjectInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ProjectInsurance.Controllers
{
    [Authorize] 
    public class InsuranceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public InsuranceController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        //Insurance/Create/id
        //GET
        public IActionResult Create(int? userId)
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
            //prevents from creating any Insurance under any user, redirecting to "error" page
            if (insuredFromDb.UserId != _userManager.GetUserId(User))
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
        public IActionResult Create(InsuranceModel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Insurance.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Pojištění uloženo.";
                return RedirectToAction("PolicyHolderDetail", "PolicyHolderDetail", new { id = obj.InsuranceHolderId });
            }
            return View(obj);
        }
        //Insurance/Edit/id
        //GET
        public IActionResult Edit(int? id)
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

            ViewBag.Name = insuredFromDb.Name;
            ViewBag.LastName = insuredFromDb.LastName;
            return View(insuranceFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InsuranceModel obj)
        {
            if (ModelState.IsValid)
            {
                _db.Insurance.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Pojištění aktualizováno.";
                return RedirectToAction("PolicyHolderDetail", "PolicyHolderDetail", new { id = obj.InsuranceHolderId });
            }
            return View(obj);
        }
        //Insurance/Delete/id
        //GET
        public IActionResult Delete(int? id)
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

            ViewBag.Name = insuredFromDb.Name;
            ViewBag.LastName = insuredFromDb.LastName;
            return View(insuranceFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Insurance.Find(id);
            if (obj == null)
            {
                return RedirectToAction("NotFoundCustom", "Home");
            }
            _db.Insurance.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Pojištění odstraněno.";
            return RedirectToAction("PolicyHolderDetail", "PolicyHolderDetail", new { id = obj.InsuranceHolderId });
        }
    }
}
