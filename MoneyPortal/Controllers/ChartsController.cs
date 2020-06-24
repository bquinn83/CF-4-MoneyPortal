using KillBug.ViewModels;
using MoneyPortal.Models;
using MoneyPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KillBug.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Budgets
        public JsonResult BudgetsBar(int householdId)
        {
            var data = new ChartData();
            data.KeyLabel = "";
            foreach(var budget in db.Categories.Where(c => c.HouseholdId == householdId).ToList())
            {
                data.Labels.Add(budget.Name);
                var now = DateTime.Now;
                var monthlyTransactions = db.Transactions.Where(t => t.Created.Year == now.Year && t.Created.Month == now.Month).Include(t => t.CategoryItem).ToList();
                monthlyTransactions = monthlyTransactions.Where(t => t.CategoryItemId != null).ToList();
                var monthlyBudgetTransactions = monthlyTransactions.Where(t => t.CategoryItem.CategoryId == budget.Id).ToList();
                var budgetCurrentAmount = (decimal)0;
                if (monthlyBudgetTransactions.Count() > 0)
                {
                    budgetCurrentAmount = monthlyBudgetTransactions.Sum(t => t.Amount);
                }
                data.Data.Add(budgetCurrentAmount);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //GET: BudgetsBreakdown
        public JsonResult BudgetsBreakdownPie(int budgetId)
        {
            var data = new ChartData();
            var budget = db.Categories.Find(budgetId);
            data.KeyLabel = budget.Name;
            foreach(var item in budget.CategoryItems.ToList())
            {
                DateTime now = DateTime.Now;
                var transactions = db.Transactions.Where(t => t.Created.Year == now.Year && t.Created.Month == now.Month);
                decimal total = 0;
                if(transactions.Any(t => t.CategoryItemId == item.Id))
                {
                    total = transactions.Where(t => t.CategoryItemId == item.Id).Sum(t => t.Amount);
                }
                data.Labels.Add(item.Name);
                data.Data.Add(total);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //    //GET:Ticket Priority Data
        //    public JsonResult TicketPriorityChartData()
        //    {
        //        var data = new ChartData();
        //        data.KeyLabel = "# of Tickets";
        //        foreach (var priority in db.TicketPriorities.ToList())
        //        {
        //            var dataCount = db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count();
        //            data.Labels.Add(priority.Name);
        //            data.Data.Add(dataCount);
        //        }
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    //GET:Ticket Status Data
        //    public JsonResult TicketStatusChartData()
        //    {
        //        var data = new ChartData();
        //        data.KeyLabel = "# of Tickets";
        //        foreach (var status in db.TicketStatus.ToList())
        //        {
        //            var dataCount = db.Tickets.Where(t => t.TicketStatusId == status.Id).Count();
        //            data.Labels.Add(status.Name);
        //            data.Data.Add(dataCount);
        //        }
        //        return Json(data);
        //    }
        //    //GET:Ticket Type Data
        //    public JsonResult TicketTypeChartData()
        //    {
        //        var data = new ChartData();
        //        data.KeyLabel = "# of Tickets";
        //        foreach (var type in db.TicketTypes.ToList())
        //        {
        //            var dataCount = db.Tickets.Where(t => t.TicketTypeId == type.Id).Count();
        //            data.Labels.Add(type.Name);
        //            data.Data.Add(dataCount);
        //        }
        //        return Json(data);
        //    }
    }
}