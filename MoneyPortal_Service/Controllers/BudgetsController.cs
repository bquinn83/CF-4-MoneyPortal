using Microsoft.SqlServer.Server;
using MoneyPortal_Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MoneyPortal_Service.Controllers
{
    [RoutePrefix("api/Budgets")]
    public class BudgetsController : ApiController
    {
        private ApiDbContext db = new ApiDbContext();
        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        #region ADD BUDGET
        /// <summary>
        /// Add a Budget to a Household.
        /// </summary>
        /// <param name="name">The Budget category name.</param>
        /// <param name="description">A description of the Budget categery.</param>
        /// <param name="targetAmount">The target amount of your Budget.</param>
        /// <param name="householdId">The ID number of the household recieving the new Budget.</param>
        /// <returns>Returns the Id number of the new Budget category as an integer.</returns>
        [Route("AddBudget"), HttpGet, HttpPost]
        public async Task<int> AddBudgetToHousehold(string name, string description, decimal targetAmount, int householdId)
        {
            var budget = new NewBudget
            {
                Name = name,
                Description = description,
                TargetAmount = targetAmount,
                HouseholdId = householdId
            };
            return await db.AddBudgetToHousehold(budget);
        }
        #endregion

        #region BUDGET DETAILS
        /// <summary>
        /// Get details of a budget category.
        /// </summary>
        /// <param name="budgetId">The Id of the budget you wish to retrieve.</param>
        /// <returns></returns>
        public async Task<Budget> GetBudgetDetails(int budgetId)
        {
            return await db.GetBudgetDetails(budgetId);
        }
        /// <summary>
        /// Get details of a budget category as JSON.
        /// </summary>
        /// <param name="budgetId">The Id of the budget you wish to retrieve.</param>
        /// <returns></returns>
        [Route("json")]
        public async Task<IHttpActionResult> GetBudgetDetailsAsJson(int budgetId)
        {
            return Json(await db.GetBudgetDetails(budgetId), serializerSettings);
        }
        #endregion

        #region BUDGET ITEMS
        /// <summary>
        /// Get all items in a Budget.
        /// </summary>
        /// <param name="budgetId">The Id of the budget category you wish to retrieve the items of.</param>
        /// <returns></returns>
        [Route("Items")]
        public async Task<List<BudgetItems>> GetBudgetItems(int budgetId)
        {
            return await db.GetBudgetItems(budgetId);
        }
        /// <summary>
        /// Get all items in a Budget as JSON.
        /// </summary>
        /// <param name="budgetId">The Id of the budget category you wish to retrieve the items of.</param>
        /// <returns></returns>
        [Route("Items/json")]
        public async Task<IHttpActionResult> GetBudgetItemsAsJson(int budgetId)
        {
            return Json(await db.GetBudgetItems(budgetId), serializerSettings);
        }
        #endregion

        #region BUDGET ITEM DETAILS
        /// <summary>
        /// Get the details of a budget item.
        /// </summary>
        /// <param name="budgetItemId">The Id of the budget item you wish to retrieve.</param>
        /// <returns></returns>
        [Route("ItemDetails")]
        public async Task<BudgetItemDetails> GetBudgetItemDetails(int budgetItemId)
        {
            return await db.GetBudgetItemDetails(budgetItemId);
        }
        /// <summary>
        /// Get the details of a budget item as JSON.
        /// </summary>
        /// <param name="budgetItemId">The Id of the budget item you wish to retrieve.</param>
        /// <returns></returns>
        [Route("ItemDetails/json")]
        public async Task<IHttpActionResult> GetBudgetItemDetailsAsJson(int budgetItemId)
        {
            return Json(await db.GetBudgetItemDetails(budgetItemId), serializerSettings);
        }
        #endregion

        #region DELETE BUDGET & BUDGET ITEMS
        /// <summary>
        /// Deletes a budget item.
        /// </summary>
        /// <remarks>This will delete a specific budget item.</remarks>
        /// <param name="budgetItemId">Id of the budget item.</param>
        /// <returns></returns>
        [Route("DeleteBudgetItem"), HttpDelete]
        public async Task<int> DeleteBudgetItem(int budgetItemId)
        {
            return await db.DeleteCategoryItem(budgetItemId);
        }
        /// <summary>
        /// Deletes a budget.
        /// </summary>
        /// <remarks>Deletes a budget and all associated budget Items. May not Update Transactions... we will find out.</remarks>
        /// <param name="budgetId">Id of the budget to be deleted.</param>
        /// <returns></returns>
        [Route("DeleteBudget")]
        public async Task<int> DeleteBudget(int budgetId)
        {
            return await db.DeleteCategory(budgetId);
        }
        #endregion

    }
}