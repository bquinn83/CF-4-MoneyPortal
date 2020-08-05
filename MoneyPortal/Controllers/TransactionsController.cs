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

        //POST: Transactions/AddTransaction
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

            if (transactionType == "Deposit")
            {
                account.CurrentBalance += transaction.Amount;
            }
            else if (transactionType != "Transfer")
            {
                account.CurrentBalance -= transaction.Amount;

            }
            try
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();

                //Call Notification Checker
                //noteHelper.TransactionNotification(bank, transaction);

                return Json(db.Transactions.Where(t => t.BankAccountId == transaction.BankAccountId).Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }
        // POST: Transactions/EditTransaction
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult EditTransaction(int transactionId, decimal amount, int TypeId, string memo)
        {
            var transaction = db.Transactions.Find(transactionId);
            var account = db.BankAccounts.Find(transaction.BankAccountId);
            var originalAmount = transaction.Amount;
            var originalType = transaction.TransactionType.Name;
            var newType = db.TransactionTypes.Find(TypeId).Name;

            transaction.Amount = amount;
            transaction.TransactionTypeId = TypeId;
            transaction.Memo = memo;

            db.Entry(transaction).State = EntityState.Modified;
            //undo old transaction
            if(originalType == "Deposit")
            {
                account.CurrentBalance -= originalAmount;
            } else if(originalType != "Transfer")
            {
                account.CurrentBalance += originalAmount;
            }
            //calculate new transaction
            if (newType == "Deposit")
            {
                account.CurrentBalance += transaction.Amount;
            }
            else if (newType != "Transfer")
            {
                account.CurrentBalance -= transaction.Amount;
            }
            db.Entry(account).State = EntityState.Modified;
            
            //send notification

            db.SaveChanges();

            return Json(true);
        }
        // GET: Transactions/DeleteTransaction
        [HttpPost]
        public JsonResult DeleteTransaction(int id)
        {
            var transaction = db.Transactions.Find(id);
            var transactionType = db.TransactionTypes.Find(transaction.TransactionTypeId).Name;
            var account = db.BankAccounts.Find(transaction.BankAccountId);

            if (transactionType == "Deposit")
            {
                account.CurrentBalance -= transaction.Amount;
            }
            else if (transactionType != "Transfer")
            {
                account.CurrentBalance += transaction.Amount;
            }
            try
            {
                db.Transactions.Remove(transaction);
                db.SaveChanges();
                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
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