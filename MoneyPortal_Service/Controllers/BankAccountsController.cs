using MoneyPortal_Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;

namespace MoneyPortal_Service.Controllers
{
    [RoutePrefix("api/Accounts")]
    public class BankAccountsController : ApiController
    {
        private ApiDbContext db = new ApiDbContext();
        private JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        #region ADD ACCOUNT
        /// <summary>
        /// Adds a Bank Account to a user with their Email.
        /// </summary>
        /// <param name="name">The account name.</param>
        /// <param name="accountType">The bank account type.</param>
        /// <param name="startingBalance">The starting balance of the account.  Will also be set as current balance.</param>
        /// <param name="lowBalance">The low balance alert threshold.</param>
        /// <param name="email">The email of the user the account is being added to.</param>
        /// <returns></returns>
        [Route("AddAccount")]
        public async Task<int> AddBankAccount(string name, int accountType, decimal startingBalance, decimal lowBalance, string email)
        {
            var account = new NewBankAccount
            {
                Name = name,
                Type = accountType,
                StartingBalance = startingBalance,
                LowBalanceLevel = lowBalance
            };
            return await db.CreateAccount(account, email);
        }
        #endregion

        #region ALL ACCOUNTS
        /// <summary>
        /// Returns all Bank Accounts.
        /// </summary>
        /// <returns>A list of all Bank Accounts.</returns>
        [Route("All")]
        public async Task<List<Account>> GetAllAccounts()
        {
            return await db.GetAllAccounts();
        }
        /// <summary>
        /// Returns all Bank Accounts as JSON.
        /// </summary>
        /// <returns></returns>
        [Route("All/json")]
        public async Task<IHttpActionResult> GetAllAccountsAsJson()
        {
            return Json(await db.GetAllAccounts(), serializerSettings);
        }
        #endregion

        #region ACCOUNTS FOR HOUSEHOLD
        /// <summary>
        /// Returns bank accounts for a household.
        /// </summary>
        /// <param name="householdId">The Id of household.</param>
        /// <returns></returns>
        public async Task<List<BankAccount>> GetAcounts(int householdId)
        {
            return await db.GetAccounts(householdId);
        }
        /// <summary>
        /// Returns bank accounts for a household.
        /// </summary>
        /// <param name="householdId">The Id of household.</param>
        /// <returns></returns>
        [Route("json")]
        public async Task<IHttpActionResult> GetAccountsAsJson(int householdId)
        {
            return Json(await db.GetAccounts(householdId), serializerSettings);
        }
        #endregion

        #region ACCOUNT DETAILS
        /// <summary>
        /// Get details of a bank account.
        /// </summary>
        /// <param name="accountId">The bank account Id.</param>
        /// <returns></returns>
        [Route("Details")]
        public async Task<BankAccount> GetAccountDetails(int accountId)
        {
            return await db.GetAccountDetails(accountId);
        }
        /// <summary>
        /// Get details of a bank account.
        /// </summary>
        /// <param name="accountId">The bank account Id.</param>
        /// <returns></returns>
        [Route("Details/json")]
        public async Task<IHttpActionResult> GetAccountDetailsAsJson(int accountId)
        {
            return Json(await db.GetAccountDetails(accountId), serializerSettings);
        }
        #endregion
    }
}
