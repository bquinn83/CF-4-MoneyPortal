using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoneyPortal.Classes;
using MoneyPortal.Models;

namespace MoneyPortal.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private NotificationHelper noteHelper = new NotificationHelper();

        // POST: Transactions/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BankAccountId,TransactionTypeId,CategoryItemId,Amount,Memo,Created,PurchaserId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var bank = db.BankAccounts.Find(transaction.BankAccountId);
                var transactionType = db.TransactionTypes.Find(transaction.TransactionTypeId).Name;
                switch (transactionType)
                {
                    case "Withdrawal":
                        bank.CurrentBalance -= transaction.Amount;
                        break;
                    case "Deposit":
                        bank.CurrentBalance += transaction.Amount;
                        break;
                }

                noteHelper.TransactionNotification(bank, transaction);

                return RedirectToAction("Main", "Dashboard");
            }

            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.CategoryItemId = new SelectList(db.CategoryItems, "Id", "Name", transaction.CategoryItemId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,TransactionTypeId,CategoryItemId,Amount,Memo,Created,PurchaserId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.CategoryItemId = new SelectList(db.CategoryItems, "Id", "Name", transaction.CategoryItemId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TransactionTypeId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //AJAX ACTIONS
        //POST: Transactions/Create
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult AddTransaction(int accountId, decimal amount, int TransactionTypes, string memo)
        {
            var transaction = new Transaction
            {
                BankAccountId = accountId,
                Amount = amount,
                Memo = memo,
                TransactionTypeId = TransactionTypes,
                Created = DateTime.Now,
            };
            var transactionType = db.TransactionTypes.Find(transaction.TransactionTypeId).Name;
            var account = db.BankAccounts.Find(transaction.BankAccountId);

            if(transactionType == "Deposit")
            {
                account.CurrentBalance += transaction.Amount;
            } 
            else if(transactionType != "Transfer")
            {
                account.CurrentBalance -= transaction.Amount;

            }
            try
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();

                //Call Notification Checker
                //noteHelper.TransactionNotification(bank, transaction);

                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }
    }
}