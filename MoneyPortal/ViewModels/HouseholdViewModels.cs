using MoneyPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoneyPortal.ViewModels
{
    public class HouseholdTransactionsVM
    {
        public string HouseholdName { get; set; }
        public List<TransactionVM> Transactions { get; set; }
    }
    public class TransactionVM
    {
        public int Id { get; set; }
        public string BankAccountName { get; set; }
        public ApplicationUser Owner { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Memo { get; set; }
        public string BudgetItem { get; set; }
        public SelectList BudgetItemList { get; set; }
        public DateTime Date { get; set; }
    }
    public class HouseholdMembersVM
    {
        public HouseholdMember Owner { get; set; }
        public List<HouseholdMember> Members { get; set; }
        public HouseholdMembersVM()
        {
            Members = new List<HouseholdMember>();
        }
    }

    public class HouseholdMember
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public int JoinedAccounts { get; set; }
        public int Transactions { get; set; }
    }

    public class BudgetListVM
    {
        public MultiSelectList BudgetList { get; set; }
        public decimal TotalBudget { get; set; }
    }
}