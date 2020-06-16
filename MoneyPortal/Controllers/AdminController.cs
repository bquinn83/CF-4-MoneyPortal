using MoneyPortal.Models;
using MoneyPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoneyPortal.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Admin()
        {
            var viewData = new AdminVM
            {
                BankAccountTypes = db.BankAccountTypes.ToList(),
                TransactionTypes = db.TransactionTypes.ToList()
            };

            return View(viewData);
        }

        //POST: AJAX Bank Account Type
        [HttpPost]
        public JsonResult AddBankAccountType(string type)
        {
            try
            {
                db.BankAccountTypes.Add(new BankAccountType(type));
                db.SaveChanges();

                return Json(type);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }
        //POST: AJAX Transaction Type
        public JsonResult AddTransactionType(string type)
        {
            try
            {
                db.TransactionTypes.Add(new TransactionType(type));
                db.SaveChanges();
                return Json(type);
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }
    }
}