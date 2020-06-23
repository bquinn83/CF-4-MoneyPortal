using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public Boolean IsRead { get; set; }

        public string RecipientId { get; set; }
        public int? HouseholdId { get; set; }
        public int? BankAccountId { get; set; }
        public int? CategoryId { get; set; }
    }
}