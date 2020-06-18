using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? CategoryItemId { get; set; }

        public decimal Amount { get; set; }
        public string Memo { get; set; }
        public DateTime Created { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        public virtual CategoryItem CategoryItem { get; set; }
    }
}