using Microsoft.AspNet.Identity;
using MoneyPortal.Models;
using Newtonsoft.Json;
using Sgml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MoneyPortal.ViewModels;
using CsQuery.ExtensionMethods;

namespace MoneyPortal.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //POST: Create Budget
        [HttpPost, ValidateAntiForgeryToken]
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
                foreach (var b in db.Categories.Where(c => c.HouseholdId == user.HouseholdId).ToList())
                {
                    data.Add(b.Id, b.Name);
                }
                //build list of categories
                //return to View
                return Json(JsonConvert.SerializeObject(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }

        //POST: Create Budget Item
        [HttpPost, ValidateAntiForgeryToken]
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

        //POST: Budgets/UpdateBudgetItem
        [HttpPost]
        public JsonResult UpdateBudgetItem(int transactionId, string budgetItemId)
        {
            try
            {
                var transaction = db.Transactions.Find(transactionId);
                var removal = false;
                if (budgetItemId == "")
                {
                    transaction.CategoryItemId = null;
                    removal = true;
                }
                else
                {
                    transaction.CategoryItemId = Convert.ToInt32(budgetItemId);
                }
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                if (removal)
                {
                    return Json("reset");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }

        //POST: Budgets/RemoveItem
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult RemoveItem(List<string> BudgetList)
        {
            var itemsRemoved = 0;
            try
            {
                foreach (var item in BudgetList)
                {
                    if(item != "")
                    {
                        var id = Convert.ToInt32(item);
                        db.Transactions.Where(t => t.CategoryItemId == id).ForEach(t => t.CategoryItemId = null);
                        db.CategoryItems.Remove(db.CategoryItems.Find(id));
                        db.SaveChanges();
                        itemsRemoved++;
                    }
                }
                return Json(itemsRemoved);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(false);
            }
        }

        //PARTIAL VIEW
        //GET: Budgets/BudgetList
        public ActionResult BudgetList()
        {
            var userId = db.Users.Find(User.Identity.GetUserId()).Id;
            var householdId = db.Households.Where(h => h.OwnerId == userId).First().Id;

            var items = new List<SelectListItem>();
            foreach (var budget in db.Categories.Where(c => c.HouseholdId == householdId).OrderBy(c => c.Name).ToList())
            {
                var group = new SelectListGroup() { Name = $"{ budget.Name } (${budget.TargetAmount})" };
                if (budget.CategoryItems.Count > 0)
                {
                    foreach (var budgetItem in budget.CategoryItems.OrderBy(ci => ci.Name).ToList())
                    {
                        items.Add(new SelectListItem()
                        {
                            Value = budgetItem.Id.ToString(),
                            Text = budgetItem.Name,
                            Group = group
                        });
                    }
                }
                else
                {
                    items.Add(new SelectListItem()
                    {
                        Value = "",
                        Text = " - Create An Item - ",
                        Group = group
                    });
                }
            }

            var viewData = new BudgetListVM {
                BudgetList = new MultiSelectList(items, "Value", "Text", "Group.Name"),
                TotalBudget = db.Categories.Where(c => c.HouseholdId == householdId).Sum(c => (decimal?)c.TargetAmount) ?? 0
            };
           
            return PartialView("~/Views/Households/_BudgetList.cshtml", viewData);
        }
    }
}