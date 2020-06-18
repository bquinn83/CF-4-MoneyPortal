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

        // POST: BankAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string bankAccountName, int Types, decimal startingBalance, decimal lowBalanceLevel)
        {
            var bankAccount = new BankAccount
            {
                Name = bankAccountName,
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

        // GET: BankAccounts/Delete/5
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
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Bank Account Types
        [Authorize(Roles=("Admin"))]
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
