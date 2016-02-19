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

        public DateTime Start
        {
            get { return countdownModel.Start; }
            set
            {
                countdownModel.Start = value;
                RaisePropertyChanged(nameof(End));
            }
        }

        public DateTime End
        {
            get
            {
                countdownModel.End = Start.AddHours(WorkTime).AddHours(Lunch.Hour).AddMinutes(Lunch.Minute);
                return countdownModel.End;
            }
        }

        public double WorkTime
        {
            get { return countdownModel.WorkTime; }
            set
            {
                countdownModel.WorkTime = value;
                RaisePropertyChanged(nameof(End));
            }
        }

        public DateTime Lunch
        {
            get { return countdownModel.Lunch; }
            set
            {
                countdownModel.Lunch = value;
                RaisePropertyChanged(nameof(End));
            }
        }
    }
}
