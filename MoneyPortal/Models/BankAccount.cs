﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int? HouseholdId { get; set; }
        public int TypeId { get; set; }
        public DateTime Created { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal LowBalanceLevel { get; set; }

        public virtual BankAccountType Type { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual Household HouseHold { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public BankAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
} 