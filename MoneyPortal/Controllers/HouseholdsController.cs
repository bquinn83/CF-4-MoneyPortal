using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Evernote.EDAM.UserStore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MoneyPortal.Models;
using MoneyPortal.Classes;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using System.Web.ModelBinding;

namespace MoneyPortal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // POST: Households/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string HouseholdName, string HouseholdGreeting)
        {

            if (!HouseholdName.IsNullOrEmpty() && !HouseholdGreeting.IsNullOrEmpty())
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(User.Identity.GetUserId());

                Household household = new Household
                {
                    Created = DateTime.Now,
                    Name = HouseholdName,
                    Greeting = HouseholdGreeting,
                    OwnerId = userId
                };
                db.Households.Add(household);
                db.SaveChanges();

                user.HouseholdId = household.Id;
                userManager.RemoveFromRole(userId, "Personal");
                userManager.AddToRole(userId, "Owner");

                await UserAuthorization.RefreshAuthentication(HttpContext, user);

                return RedirectToAction("Main", "Dashboard");
            }
            else
            {
                return RedirectToAction("Main", "Dashboard", new { ErrorMessage = "Your input was incomplete." });
            }

        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
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
