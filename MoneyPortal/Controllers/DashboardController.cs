using CsQuery.ExtensionMethods.Internal;
using Microsoft.AspNet.Identity;
using MoneyPortal.Models;
using MoneyPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MoneyPortal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: SideMenu
        public ActionResult SideMenuRefresh()
        {
            var viewData = new SideNavVM();
            return PartialView("~/Views/Shared/_SideMenu.cshtml", viewData);
        }

        // GET: Dashboard
        public ActionResult Main()
        {
            if (User.IsInRole("Owner") || User.IsInRole("Member"))
            {
                return RedirectToAction("Household");
            }
            else if (User.IsInRole("Personal"))
            {
                ViewBag.Types = new SelectList(db.BankAccountTypes, "Id", "Name");
                return View("Lobby");
            }
            else if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin", "Admin");
            }
            return RedirectToAction("CustomLogOff", "Account");
        }

        //GET: Lobby Dashboard
        public ActionResult Lobby()
        {
            if (User.IsInRole("Personal"))
            {
                ViewBag.Types = new SelectList(db.BankAccountTypes, "Id", "Name");
                return View();
            }
            return RedirectToAction("Main");
        }

        //GET: Household Dashboard
        public ActionResult Household()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var householdId = (int)user.HouseholdId;

            var viewData = new HouseholdVM
            {
                HouseholdId = householdId,
                HouseholdName = db.Households.Find(householdId).Name,
                UsersBankAccounts = new SelectList(db.BankAccounts.Where(ba => ba.OwnerId == user.Id && ba.HouseholdId != householdId), "Id", "DisplayName"),
                Budgets = new SelectList(db.Categories.Where(c => c.HouseholdId == user.HouseholdId), "Id", "Name")
            };
            return View(viewData);
        }

        //GET: Bank Accounts Dashboard
        public ActionResult BankAccounts()
        {
            return View();
        }
    }
}