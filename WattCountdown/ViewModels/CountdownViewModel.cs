using Abb.Cz.Apps.WattCountdown.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Abb.Cz.Apps.WattCountdown.ViewModels
{
    public class CountdownViewModel : ViewModelBase
    {
        #region Fields
        private CountdownModel countdownModel = new CountdownModel();
        private static DispatcherTimer timer = new DispatcherTimer();
        #endregion

        #region Properties
        public TimeSpan StartTime
        {
            get { return countdownModel.StartTime; }
            set { countdownModel.StartTime = value; }
        }

        public TimeSpan Lunch
        {
            get { return countdownModel.Lunch; }
            set { countdownModel.Lunch = value; }
        }

        public double WorkTime
        {
            get { return countdownModel.WorkTime; }
            set { countdownModel.WorkTime = value; }
        }

        public TimeSpan TimeToGo => countdownModel.EndDate - DateTime.Now;

        public string TimeToGoLabel => TimeToGo.ToString("HH:mm:ss");
        #endregion

        #region Commands
        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }
        #endregion

        public CountdownViewModel()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            StartCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
        }

        private void Start()
        {
            var today = DateTime.Today;
            countdownModel.EndDate = today.Add(StartTime).Add(Lunch).AddHours(WorkTime);
            timer.Start();
        }

        private void Stop()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(TimeToGoLabel));
        }
    }
}
