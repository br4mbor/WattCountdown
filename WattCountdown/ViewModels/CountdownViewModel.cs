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
        public DateTime Start
        {
            get { return countdownModel.Start; }
            set { countdownModel.Start = value; }
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
                //if (value >= 6 && !LunchVoucher)
                //    LunchVoucher = true;
                //else if (value < 6 && LunchVoucher)
                //    LunchVoucher = false;
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

        public string CountdownLabel
        {
            get { return _countdownLabel; }
            set
            {
                _countdownLabel = value;
                RaisePropertyChanged(nameof(CountdownLabel));
            }
        }

        private bool _startEnabled = true;
        private bool _workTimeEnabled = true;
        private System.Windows.Visibility _countdownVisible = System.Windows.Visibility.Hidden;
        private bool _startButtonEnabled = true;
        private bool _stopButtonEnabled = false;
        private string _countdownLabel;

        public bool StartEnabled
        {
            get { return _startEnabled; }
            private set
            {
                _startEnabled = value;
                RaisePropertyChanged(nameof(StartEnabled));
            }
        }

        public bool WorkTimeEnabled
        {
            get { return _workTimeEnabled; }
            private set
            {
                _workTimeEnabled = value;
                RaisePropertyChanged(nameof(WorkTimeEnabled));
            }
        }

        public System.Windows.Visibility CountdownVisible
        {
            get { return _countdownVisible; }
            private set
            {
                _countdownVisible = value;
                RaisePropertyChanged(nameof(CountdownVisible));
            }
        }

        public bool StartButtonEnabled
        {
            get { return _startButtonEnabled; }
            private set
            {
                _startButtonEnabled = value;
                RaisePropertyChanged(nameof(StartButtonEnabled));
            }
        }

        public bool StopButtonEnabled
        {
            get { return _stopButtonEnabled; }
            private set
            {
                _stopButtonEnabled = value;
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

            StartCommand = new RelayCommand(StartCountdown);
            StopCommand = new RelayCommand(StopCountdown);
        }

        private void StartCountdown()
        {
            LockInterface();
            var now = DateTime.Now;
            countdownModel.EndDate = now.Date.Add(Start.TimeOfDay).Add(Lunch).AddHours(WorkTime);
            Countdown = countdownModel.EndDate - now;
            timer.Start();
        }

        private void StopCountdown()
        {
            timer.Stop();
            UnlockInterface();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Countdown = Countdown.Subtract(timer.Interval);

            if (Countdown.TotalSeconds < 0)
            {
                timer.Stop();
                CountdownLabel = Properties.Resources.CountdownFinished;
            }
            else
            {
                CountdownLabel = Countdown.ToString(@"hh\:mm\:ss");
            }
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
