using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyPortal.Services
{
    public interface IMailTemplate
    {
        string CodeUrl { get; set; }
    }
}