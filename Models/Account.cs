using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class Account
    {
        public string userName { get; set; }
        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}