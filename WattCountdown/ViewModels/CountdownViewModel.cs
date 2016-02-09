using Abb.Cz.Apps.WattCountdown.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abb.Cz.Apps.WattCountdown.ViewModels
{
    public class CountdownViewModel : ViewModelBase
    {
        private CountdownModel countdownModel = new CountdownModel();

        public double WorkTime
        {
            get { return countdownModel.WorkTime; }
            set { countdownModel.WorkTime = value; }
        }
    }
}
