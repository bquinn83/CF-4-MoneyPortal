using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MoneyPortal.Models;

namespace MoneyPortal.Controllers
{
    [Authorize]
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: BankAccounts/AccountDetails
        public ActionResult AccountDetails(int accountId)
        {
            var account = db.BankAccounts.Find(accountId);
            ViewBag.BankAccountTypes = new SelectList(db.BankAccounts.ToList(), "Id", "Name");
            ViewBag.TransactionTypes = new SelectList(db.TransactionTypes.ToList(), "Id", "Name");
            return View(account);
        }

        //POST: BankAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string bankAccountName, int Types, decimal startingBalance, decimal lowBalanceLevel)
        {
            var bankAccount = new BankAccount
            {
                Name = bankAccountName,
                DisplayName = $"{bankAccountName} - {db.BankAccountTypes.Where(bat => bat.Id == Types).First().Name}",
                TypeId = Types,
                Created = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                StartingBalance = startingBalance,
                CurrentBalance = startingBalance,
                LowBalanceLevel = lowBalanceLevel
            };

            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Main", "Dashboard");
        }

        //GET: BankAccounts/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            db.BankAccounts.Remove(bankAccount);
            db.SaveChanges();

            return RedirectToAction("Main", "Dashboard");
        }



        // POST: Bank Account Types
        [Authorize(Roles = ("Admin"))]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateType(string name)
        {
            if (name != null)
            {
                var type = new BankAccountType { Name = name };
                db.BankAccountTypes.Add(type);
                db.SaveChanges();

                return RedirectToAction("Main", "Dashboard");
            }
            return RedirectToAction("Main", "Dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
