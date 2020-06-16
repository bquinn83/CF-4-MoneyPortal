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
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : ApiController
    {
        private readonly ApiDbContext db = new ApiDbContext();
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        #region TRANSACTIONS FOR ACCOUNT
        /// <summary>
        /// Get all transactions from a bank account.
        /// </summary>
        /// <param name="accountId">The Id of the bank account.</param>
        /// <returns>A list of transactions from a specific bank account.</returns>
        [Route("FromAccount")]
        public async Task<List<Transactions>> GetTransactions(int accountId)
        {
            return await db.GetTransactions(accountId);
        }
        /// <summary>
        /// Get all transactions from a bank accountin JSON format.
        /// </summary>
        /// <param name="accountId">The Id of the bank account.</param>
        /// <returns>A list of transactions from a specific bank account.</returns>
        [Route("FromAccount/json")]
        public async Task<IHttpActionResult> GetTransactionsAsJson(int accountId)
        {
            return Json(await db.GetTransactions(accountId), serializerSettings);
        }
        #endregion

        #region TRANSACTION DETAILS
        /// <summary>
        /// Get the details of a transaction.
        /// </summary>
        /// <param name="transactionId">The Id of the transaction being retrieved.</param>
        /// <returns>A single transactins details.</returns>
        [Route("Details")]
        public async Task<TransactionDetails> GetTransactionDetails(int transactionId)
        {
            return await db.GetTransactionDetails(transactionId);
        }
        /// <summary>
        /// Get the details of a transaction in JSON format.
        /// </summary>
        /// <param name="transactionId">The Id of the transaction being retrieved.</param>
        /// <returns>A single transactions details.</returns>
        [Route("Details/json")]
        public async Task<IHttpActionResult> GetTransactionDetailsAsJson(int transactionId)
        {
            return Json(await db.GetTransactionDetails(transactionId), serializerSettings);
        }
        #endregion

        #region ADD TRANSACTION
        /// <summary>
        /// Add a transaction to a bank account.
        /// </summary>
        /// <param name="amount">Amount of the transaction.</param>
        /// <param name="memo">Details about the transaction.</param>
        /// <param name="transactionTypeId">The Id of the transaction type.</param>
        /// <param name="bankAccountId">The Id of the bank account recieving transaction.</param>
        /// <returns>The Id of the new transaction.</returns>
        [Route("Add"), HttpGet, HttpPost]
        public async Task<int> AddTransactionToBankAccount(decimal amount, string memo, int transactionTypeId, int bankAccountId)
        {
            var transaction = new NewTransaction
            {
                Amount = amount,
                Memo = memo,
                TransactionTypeId = transactionTypeId,
                BankAccountId = bankAccountId
            };
            return await db.AddTransaction(transaction);
        }
        #endregion

        #region UPDATE TRANSACTION CATEGORY
        /// <summary>
        /// Update transaction category.
        /// </summary>
        /// <param name="transactionId">Id of transaction being updated.</param>
        /// <param name="categoryItemId">Id of category item being assigned to transaction.</param>
        /// <returns></returns>
        [Route("UpdateCategory"), HttpPut]
        public async Task<int> UpdateTransactionCategory(int transactionId, int categoryItemId)
        {
            return await db.UpdateTransactionCategory(transactionId, categoryItemId);
        }
        #endregion
    }
}