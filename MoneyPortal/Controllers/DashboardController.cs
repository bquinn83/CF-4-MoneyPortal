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
        // GET: Dashboard
        public ActionResult Main()
        {
            //DECIDE WHAT TO VIEW
            //IF OWNER/MEMBER SHOW HOUSEHOLD
            //IF NEITHER SHOW CREATE HOUSE/ADD BANK ACCOUNT

            if (User.IsInRole("Owner") || User.IsInRole("Member"))
            {
                //GO TO HOUSEHOLD DASH
            }
            else if (User.IsInRole("Personal"))
            {
                //GO TO LOBBY DASHBOARD
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
            return View();
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
}