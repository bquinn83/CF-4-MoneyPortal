using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MoneyPortal.Models;
namespace MoneyPortal.ViewModels
{
    public class AccountStatistics
    {
        public string CurrentBalance { get; set; }
        public string MonthlySpending { get; set; }
        public string MonthlyDeposits { get; set; }
        public int MonthlyTransactions { get; set; }

        public AccountStatistics(BankAccount account)
        {
            CurrentBalance = account.CurrentBalance.ToString("C");
            
            var spending = (decimal)account.Transactions
                                .Where(t => t.Created.Year == DateTime.Now.Year && t.Created.Month == DateTime.Now.Month)
                                .Where(t => t.TransactionType.Name != "Deposit" || t.TransactionType.Name != "Transfer")
                                .Sum(t => (decimal?)t.Amount);
            MonthlySpending = spending.ToString("c");
            var deposits = (decimal)account.Transactions
                                    .Where(t => t.Created.Year == DateTime.Now.Year && t.Created.Month == DateTime.Now.Month)
                                    .Where(t => t.TransactionType.Name == "Deposit")
                                    .Sum(t => (decimal?)t.Amount);
            MonthlyDeposits = deposits.ToString("c");
            MonthlyTransactions = account.Transactions
                                    .Where(t => t.Created.Year == DateTime.Now.Year && t.Created.Month == DateTime.Now.Month)
                                    .Count();
        }
    }
}