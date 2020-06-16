using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MoneyPortal_Service.Models
{
    public class User
    {
        public string Id { get; set; }
    }
    public class Household
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Greeting { get; set; }
        [DisplayName("Owner First Name")]
        public string FirstName { get; set; }
        [DisplayName("Owner Last Name")]
        public string LastName { get; set; }
    }
    public class HouseholdBudgets
    {
        public int Id { get; set; }
        public string HouseholdName { get; set; }
        public string BudgetName { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
    }
    public class NewBudget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public int HouseholdId { get; set; }
    }
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HouseholdName { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
    }
    public class BudgetItems
    {
        public int Id { get; set; }
        public string BudgetName { get; set; }
        public string Name { get; set; }
    }
    public class BudgetItemDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string HouseholdName { get; set; }
    }
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string OwnerFirst { get; set; }
        public string OwnerLast { get; set; }
        public string OwnerEmail { get; set; }

    }
    public class NewBankAccount
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal LowBalanceLevel { get; set; }
        public string OwnerId { get; set; }
    }
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal LowBalanceLevel { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HouseholdName { get; set; }
    }
    public class Transactions
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }
    public class TransactionDetails
    {
        public int Id { get; set; }
        public string BankAccountName { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Memo { get; set; }
        public DateTime Created { get; set; }
        public string CategoryItem { get; set; }
    }
    public class NewTransaction
    {
        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public int TransactionTypeId { get; set; }
        public int BankAccountId { get; set; }
    }
}