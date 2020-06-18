using CsQuery.ExtensionMethods.Internal;
using MoneyPortal.Models;
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

        // GET: Dashboard
        public ActionResult Main(string Message, bool? success)
        {
            if (!Message.IsNullOrEmpty())
                ViewBag.Message = Message;

            ViewBag.Success = null;
            if (success != null)
                ViewBag.Success = success;

            if (User.IsInRole("Owner") || User.IsInRole("Member"))
            {
                return View("Household");
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
            return View();
        }

        //GET: Bank Accounts Dashboard
        public ActionResult BankAccounts()
        {
            return View();
        }
    }

    //ViewModels
    public class LobbyVM 
    {
        public IEnumerable<SelectListItem> Types { get; set; }
    }
}