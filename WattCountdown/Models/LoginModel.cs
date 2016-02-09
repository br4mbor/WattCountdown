using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        //public string ProxyUserName { get; set; }

        //public string ProxyPassword { get; set; }

        //public string ProxyAddress { get; set; } = "proxy.cz.abb.com";

        //public int ProxyPort { get; set; } = 8080;

        //public bool UseProxy { get; set; }
    }
}
