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
using MoneyPortal.ViewModels;

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
            ViewBag.BankAccountTypes = new SelectList(db.BankAccountTypes.ToList(), "Id", "Name", account.TypeId);
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

        //POST: BankAccounts/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string bankAccountName, int BankAccountTypes, decimal lowBalanceLevel)
        {
            var account = db.BankAccounts.Find(id);
            account.Name = bankAccountName;
            account.TypeId = BankAccountTypes;
            account.LowBalanceLevel = lowBalanceLevel;
            account.DisplayName = $"{ account.Name } - { account.Type.Name }";
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AccountDetails", new { accountId = id });
        }

        //GET: BankAccounts/Delete
        public ActionResult Delete(int accountId)
        {
            BankAccount bankAccount = db.BankAccounts.Find(accountId);
            if (bankAccount == null)
            {
                return RedirectToAction("Main", "Dashboard");
            }

            foreach(var transaction in bankAccount.Transactions.ToList())
            {
                db.Transactions.Remove(transaction);
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

        //PARTIAL VIEW ACTIONS
        //GET: BankAccounts/AccountStatistics
        public ActionResult AccountStatistics(int id)
        {
            var account = db.BankAccounts.Find(id);
            return PartialView("_AccountStatistics", new AccountStatistics(account));
        }
        //GET: BankAccounts/AccountTransactions
        public ActionResult AccountTransactions(int id)
        {
            var account = db.BankAccounts.Find(id);
            return PartialView("_Transactions", account);
        }
    }
}
