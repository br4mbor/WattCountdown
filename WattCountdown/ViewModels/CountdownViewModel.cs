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
            set
            {
                countdownModel.WorkTime = value;
                if (value >= 6 && !LunchVoucher)
                    LunchVoucher = true;
                else if (value < 6 && LunchVoucher)
                    LunchVoucher = false;
            }
        }

        public bool LunchVoucher
        {
            get { return countdownModel.LunchVoucher; }
            private set
            {
                countdownModel.LunchVoucher = value;
                RaisePropertyChanged(nameof(LunchVoucher));
            }
        }

        public TimeSpan Countdown
        {
            get { return countdownModel.Countdown; }
            private set
            {
                countdownModel.Countdown = value;
                RaisePropertyChanged(nameof(Countdown));
            }
        }

        private bool startEnabled = true;
        private bool workTimeEnabled = true;
        private System.Windows.Visibility countdownVisible = System.Windows.Visibility.Hidden;
        private bool startButtonEnabled = true;
        private bool stopButtonEnabled = false;

        public bool StartEnabled
        {
            get { return startEnabled; }
            private set
            {
                startEnabled = value;
                RaisePropertyChanged(nameof(StartEnabled));
            }
        }

        public bool WorkTimeEnabled
        {
            get { return workTimeEnabled; }
            private set
            {
                workTimeEnabled = value;
                RaisePropertyChanged(nameof(WorkTimeEnabled));
            }
        }

        public System.Windows.Visibility CountdownVisible
        {
            get { return countdownVisible; }
            private set
            {
                countdownVisible = value;
                RaisePropertyChanged(nameof(CountdownVisible));
            }
        }

        public bool StartButtonEnabled
        {
            get { return startButtonEnabled; }
            private set
            {
                startButtonEnabled = value;
                RaisePropertyChanged(nameof(StartButtonEnabled));
            }
        }

        public bool StopButtonEnabled
        {
            get { return stopButtonEnabled; }
            private set
            {
                stopButtonEnabled = value;
                RaisePropertyChanged(nameof(StopButtonEnabled));
            }
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
            LockInterface();
            var now = DateTime.Now;
            countdownModel.EndDate = now.Date.Add(StartTime).Add(Lunch).AddHours(WorkTime);
            Countdown = countdownModel.EndDate - now;
            timer.Start();
        }

        private void Stop()
        {
            timer.Stop();
            UnlockInterface();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Countdown = Countdown.Subtract(timer.Interval);
        }

        private void LockInterface()
        {
            StartEnabled = false;
            WorkTimeEnabled = false;
            CountdownVisible = System.Windows.Visibility.Visible;
            StartButtonEnabled = false;
            StopButtonEnabled = true;
        }

        private void UnlockInterface()
        {
            StartEnabled = true;
            WorkTimeEnabled = true;
            CountdownVisible = System.Windows.Visibility.Hidden;
            StartButtonEnabled = true;
            StopButtonEnabled = false;
        }
    }
}
