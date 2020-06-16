using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Models
{
    public class BankAccountType
    {
        public int Id { get; set;}
        public string Name { get; set; }

        public BankAccountType() { }
        public BankAccountType(string type)
        {
            Name = type;
        }

    }
}