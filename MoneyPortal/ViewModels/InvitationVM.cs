using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.ViewModels
{
    public class InvitationVM
    {
        public string SenderId { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}