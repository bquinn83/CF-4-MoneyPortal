using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TransactionType() { }
        public TransactionType(string type)
        {
            Name = type;
        }
    }
}