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

        public TimeSpan Countdown
        {
            get; set;
        }

        public TimeSpan TimeToGo => countdownModel.EndDate - DateTime.Now;

        public string TimeToGoLabel => TimeToGo.ToString(@"hh\:mm\:ss");

        private bool startEnabled = true;
        private bool lunchEnabled = true;
        private bool workTimeEnabled = true;
        private System.Windows.Visibility timeToGoVisible = System.Windows.Visibility.Hidden;

        public bool StartEnabled
        {
            get { return startEnabled; }
            private set { startEnabled = value; RaisePropertyChanged(nameof(StartEnabled)); }
        }

        public bool LunchEnabled
        {
            get { return lunchEnabled; }
            private set { lunchEnabled = value; RaisePropertyChanged(nameof(LunchEnabled)); }
        }

        public bool WorkTimeEnabled
        {
            get { return workTimeEnabled; }
            private set { workTimeEnabled = value; RaisePropertyChanged(nameof(WorkTimeEnabled)); }
        }

        public System.Windows.Visibility TimeToGoVisible
        {
            get { return timeToGoVisible; }
            private set { timeToGoVisible = value; RaisePropertyChanged(nameof(TimeToGoVisible)); }
        }

        #endregion

        #region Commands
        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }
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
            var now = DateTime.Now;
            countdownModel.EndDate = now.Date.Add(StartTime).Add(Lunch).AddHours(WorkTime);
            Countdown = countdownModel.EndDate - now;
            LockInterface();
            timer.Start();
        }

        private void Stop()
        {
            UnlockInterface();
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(TimeToGoLabel));
        }

        private void LockInterface()
        {
            StartEnabled = false;
            LunchEnabled = false;
            WorkTimeEnabled = false;
            TimeToGoVisible = System.Windows.Visibility.Visible;
        }

        private void UnlockInterface()
        {
            StartEnabled = true;
            LunchEnabled = true;
            WorkTimeEnabled = true;
            TimeToGoVisible = System.Windows.Visibility.Hidden;
        }
    }
}
