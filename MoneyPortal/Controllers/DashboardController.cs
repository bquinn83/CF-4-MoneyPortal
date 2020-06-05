using MoneyPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoneyPortal.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Main()
        {
            //var types = List<string>;
            //ViewBag.BankAccountTypes = new SelectList(BankAccountTypes);
            return View();
        }
    }
}