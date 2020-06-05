using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public Guid Code { get; set; }
        public DateTime Created { get; set; }
        public virtual Household Household { get; set; }
    }
}