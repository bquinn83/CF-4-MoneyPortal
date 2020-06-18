using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoneyPortal.ViewModels
{
    public class HouseholdVM
    {
        public int HouseholdId { get; set; }
        public SelectList UsersBankAccounts { get; set; }
        public SelectList Budgets { get; set; }
    }
    public class LobbyVM
    {
        public SelectList Types { get; set; }
    }

    public class BudgetsSelectList
    {
        public SelectList Budgets { get; set; }
    }
}