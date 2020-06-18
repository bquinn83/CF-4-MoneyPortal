using Microsoft.AspNet.Identity;
using MoneyPortal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoneyPortal.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //POST: Create Budget
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateBudget(string budgetName, string budgetDescription, decimal budgetAmount)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var budget = new Category
            {
                Name = budgetName,
                Description = budgetDescription,
                TargetAmount = budgetAmount,
                HouseholdId = db.Households.Find(user.HouseholdId).Id
            };

            try
            {
                db.Categories.Add(budget);
                db.SaveChanges();

                var data = new Dictionary<int, string>();
                foreach(var b in db.Categories.ToList())
                {
                    data.Add(b.Id, b.Name);
                }
                //build list of categories
                //return to View
                return Json(JsonConvert.SerializeObject(data), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }

        //POST: Create Budget Item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateBudgetItem(string budgetItemName, int Budgets)
        {
            var budgetItem = new CategoryItem
            {
                Name = budgetItemName,
                CategoryId = Budgets
            };
            try
            {
                db.CategoryItems.Add(budgetItem);
                db.SaveChanges();

                return Json(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }

        //GET: Budgets SelectList

    }
}