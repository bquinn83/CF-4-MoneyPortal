using MoneyPortal.Models;
using MoneyPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.ViewModels
{
    public class AdminVM
    {
        public List<BankAccountType> BankAccountTypes { get; set; } 
        public List<TransactionType> TransactionTypes { get; set; }
    }
}