using Abb.Cz.Apps.WattCountdown.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.Services
{
    class LocalSettingsService : ISettingsService
    {
        public string UserName
        {
            get
            {
                return Properties.Settings.Default.UserName;
            }

            set
            {
                Properties.Settings.Default.UserName = value;
            }
        }

        public string Password
        {
            get
            {
                return Properties.Settings.Default.Password;
            }

            set
            {
                Properties.Settings.Default.Password = value;
            }
        }
    }
}
