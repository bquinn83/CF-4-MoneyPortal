using MoneyPortal_Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace MoneyPortal_Service.Controllers
{
    [RoutePrefix("api/Households")]
    public class HouseholdsController : ApiController
    {
        private ApiDbContext db = new ApiDbContext();
        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        /// <summary>
        /// Get data for a specific household.
        /// </summary>
        /// <param name="householdId">The Id of the household you wish to retrieve.</param>
        /// <returns>A set of household data.</returns>
        [Route("Data")]
        public async Task<Household> GetHouseholdData(int householdId)
        {
            return await db.GetHouseholdData(householdId);
        }
        /// <summary>
        /// Get data for a specific household as JSON.
        /// </summary>
        /// <param name="householdId">The Id of the household you wish to retrieve.</param>
        /// <returns></returns>
        [Route("Data/json")]
        public async Task<IHttpActionResult> GetHouseholdDataAsJson(int householdId)
        {
            var json = JsonConvert.SerializeObject(await db.GetHouseholdData(householdId));
            return Ok(json);
        }
        /// <summary>
        /// Get all Budget categories for a household.
        /// </summary>
        /// <param name="hhId">The Id of the household.</param>
        /// <returns></returns>
        [Route("Budgets")]
        public async Task<List<HouseholdBudgets>> GetBudgetsForHousehold(int hhId)
        {
            return await db.GetBudgets(hhId);
        }
        /// <summary>
        /// Get all Budget categories for a household as JSON.
        /// </summary>
        /// <param name="hhId">The Id of the household.</param>
        /// <returns></returns>
        [Route("Budgets/json")]
        public async Task<IHttpActionResult> GetBudgetsForHouseholdAsJson(int hhId)
        {
            var data = await db.GetBudgets(hhId);
            return Json(data, serializerSettings);
        }
        #region CREATE HOUSEHOLD
        /// <summary>
        /// Creates a household.
        /// </summary>
        /// <param name="name">The name for the household.</param>
        /// <param name="greeting">The greeting of the household.</param>
        /// <param name="email">The email to set the owner of the household.</param>
        /// <returns></returns>
        [Route("Create"), HttpGet, HttpPost]
        public async Task<int> CreateHousehold(string name, string greeting, string email)
        {
            var house = new Household { Name = name, Greeting = greeting };
            return await db.CreateHousehold(house, email);
        }
        #endregion
    }
}