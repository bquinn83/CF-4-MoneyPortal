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
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string bankAccountName, int bankAccountType, decimal startingBalance, decimal lowBalanceLevel)
        {
            var bankAccount = new BankAccount
            {
                Name = bankAccountName,
                TypeId = bankAccountType,
                Created = DateTime.Now,
                OwnerId = User.Identity.GetUserId(),
                StartingBalance = startingBalance,
                CurrentBalance = startingBalance,
                LowBalanceLevel = lowBalanceLevel
            };

            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Created,Type,StartingBalance,CurrentBalance,LowBalanceLevel,OwnerId,HouseholdId")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseholdId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", bankAccount.OwnerId);
            return View(bankAccount);
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
