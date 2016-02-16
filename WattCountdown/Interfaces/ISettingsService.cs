using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Interfaces
{
    interface ISettingsService
    {
        string UserName { get; set; }

        string Password { get; set; }
    }
}
