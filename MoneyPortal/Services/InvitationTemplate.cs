using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Services
{
    public class InvitationTemplate : IMailTemplate
    {
        public string CodeUrl { get; set; }
        public string InviteCode { get; set; }
        public string Message { get; set; } = "";
        public InvitationTemplate(string codeUrl, string message, string code)
        {
            CodeUrl = codeUrl;
            Message = message;
            InviteCode = code;
        }
    }
}