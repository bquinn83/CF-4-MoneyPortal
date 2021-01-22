using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace MoneyPortal_Service.Models
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext()
            : base("demo-financialportal-secret")
        {

        }

        #region HOUSEHOLD
        //GET: Household
        public async Task<Household> GetHouseholdData(int hhId)
        {
            return await Database.SqlQuery<Household>("GetHouseHoldData @hhId",
                new SqlParameter("hhId", hhId)).FirstOrDefaultAsync();
        }
        //INSERT: Create Household
        public async Task<int> CreateHousehold(Household house, string email)
        {
            var ownerId = Database.SqlQuery<User>("GetUserId @email",
                new SqlParameter("email", email)).FirstOrDefault().Id;
            return await Database.ExecuteSqlCommandAsync("CreateHousehold @name, @greeting, @ownerId",
                new SqlParameter("name", house.Name),
                new SqlParameter("greeting", house.Greeting),
                new SqlParameter("ownerId", ownerId));
        }
        #endregion

        #region BUDGETS
        //GET: Household Budgets
        public async Task<List<HouseholdBudgets>> GetBudgets(int hhId)
        {
            return await Database.SqlQuery<HouseholdBudgets>("GetBudgets @hhId",
                new SqlParameter("hhId", hhId)).ToListAsync();
        }

        //GET: Budgets
        public async Task<Budget> GetBudgetDetails(int budgetId)
        {
            return await Database.SqlQuery<Budget>("GetBudgetDetails @budgetId",
                new SqlParameter("budgetId", budgetId)).FirstOrDefaultAsync();
        }

        //GET: BudgetItems
        public async Task<List<BudgetItems>> GetBudgetItems(int budgetId)
        {
            return await Database.SqlQuery<BudgetItems>("GetBudgetItems @budgetId",
                new SqlParameter("budgetId", budgetId)).ToListAsync();
        }

        //GET: Budget Item Details
        public async Task<BudgetItemDetails> GetBudgetItemDetails(int budgetItemId)
        {
            return await Database.SqlQuery<BudgetItemDetails>("GetBudgetItemDetails @budgetItemId",
                new SqlParameter("budgetItemId", budgetItemId)).FirstOrDefaultAsync();
        }
        //DELETE: Category
        public async Task<int> DeleteCategory(int categoryId)
        {
            return await Database.ExecuteSqlCommandAsync("DeleteCategory @categoryId",
                new SqlParameter("categoryId", categoryId));
        }
        //DELETE: CategoryItem
        public async Task<int> DeleteCategoryItem(int categoryItemId)
        {
            return await Database.ExecuteSqlCommandAsync("DeleteCategoryItem @categoryItemId",
                new SqlParameter("categoryItemId", categoryItemId));
        }
        #endregion

        #region ADD BUDGET
        //POST: Add Budget to Household
        public async Task<int> AddBudgetToHousehold(NewBudget budget)
        {
            var param = new SqlParameter
            {
                ParameterName = "id",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };
            await Database.ExecuteSqlCommandAsync("AddBudget @name, @description, @targetAmount, @householdId, @id OUTPUT",
                new SqlParameter("name", budget.Name),
                new SqlParameter("description", budget.Description),
                new SqlParameter("targetAmount", budget.TargetAmount),
                new SqlParameter("householdId", budget.HouseholdId),
                param
                );
            return (int)param.Value;
        }
        #endregion

        #region BANK ACCOUNTS
        //GET: All Bank Accounts
        public async Task<List<Account>> GetAllAccounts()
        {
            return await Database.SqlQuery<Account>("GetAllAccounts").ToListAsync();
        }
        //GET: Bank Accounts For Household
        public async Task<List<BankAccount>> GetAccounts(int householdId)
        {
            return await Database.SqlQuery<BankAccount>("GetAccounts @householdId",
                new SqlParameter("householdId", householdId)).ToListAsync();
        }
        //GET: Bank Account Details
        public async Task<BankAccount> GetAccountDetails(int accountId)
        {
            return await Database.SqlQuery<BankAccount>("GetAccountDetails @accountId",
                new SqlParameter("accountId", accountId)).FirstOrDefaultAsync();
        }
        //POST: Create Bank Account
        public async Task<int> CreateAccount(NewBankAccount account, string email)
        {
            var ownerId = Database.SqlQuery<User>("GetUserId @email",
                new SqlParameter("email", email)).FirstOrDefault().Id;
            return await Database.ExecuteSqlCommandAsync("CreateAccount @name, @accountType, @startingBalance, @lowBalance, @ownerId",
                new SqlParameter("name", account.Name),
                new SqlParameter("accountType", account.Type),
                new SqlParameter("startingBalance", account.StartingBalance),
                new SqlParameter("lowBalance", account.LowBalanceLevel),
                new SqlParameter("ownerId", ownerId));
        }
        #endregion

        #region TRANSACTIONS
        //GET: Transactions For BankAccount
        public async Task<List<Transactions>> GetTransactions(int accountId)
        {
            return await Database.SqlQuery<Transactions>("GetTransactions @accountId",
                new SqlParameter("accountId", accountId)).ToListAsync();
        }
        //GET: Transaction Details
        public async Task<TransactionDetails> GetTransactionDetails(int transactionId)
        {
            return await Database.SqlQuery<TransactionDetails>("GetTransactionDetails @transactionId",
                new SqlParameter("transactionId", transactionId)).FirstOrDefaultAsync();
        }
        //POST: Transaction to Bank Account
        /// <summary>
        /// ExecuteSqlCommandAsync to call "AddTransaction" stored procedure.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<int> AddTransaction(NewTransaction transaction)
        {
            var outParam = new SqlParameter
            {
                ParameterName = "id",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };
            await Database.ExecuteSqlCommandAsync("AddTransaction @amount, @memo, @transactionTypeId, @bankAccountId, @id OUT",
                new SqlParameter("amount", transaction.Amount),
                new SqlParameter("memo", transaction.Memo),
                new SqlParameter("transactionTypeId", transaction.TransactionTypeId),
                new SqlParameter("bankAccountId", transaction.BankAccountId),
                outParam);
            return (int)outParam.Value;
        }
        //UPDATE: Transaction Category
        public async Task<int> UpdateTransactionCategory(int transactionId, int categoryItemId)
        {
            return await Database.ExecuteSqlCommandAsync("UpdateTransactionCategory @transactionId, @categoryItemId",
                new SqlParameter("transactionId", transactionId),
                new SqlParameter("categoryItemId", categoryItemId));
        }
        #endregion
    }
}