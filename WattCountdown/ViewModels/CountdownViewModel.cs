using Abb.Cz.Apps.WattCountdown.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Abb.Cz.Apps.WattCountdown.Helpers;
using Abb.Cz.Apps.WattCountdown.Interfaces;
using Abb.Cz.Apps.WattCountdown.Services;
using GalaSoft.MvvmLight.Ioc;

namespace Abb.Cz.Apps.WattCountdown.ViewModels
{
    public class CountdownViewModel : ViewModelBase
    {
        #region Fields
        private CountdownModel countdownModel = new CountdownModel();
        private static DispatcherTimer timer = new DispatcherTimer();
        private TimeSpan HalfHour = TimeSpan.FromMinutes(30);
        #endregion

        #region Properties

        private DateTime _selectedDate;
        public DateTime SelectedDate {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged(nameof(SelectedDate));
            }
    }

        public DateTime Start
        {
            get { return countdownModel.Start; }
            set
            {
                countdownModel.Start = value; 
                RaisePropertyChanged(nameof(Start));

            }
        }

        public DateTime EndTime
        {
            get { return countdownModel.EndDate; }
            set
            {
                countdownModel.EndDate = value; 
                RaisePropertyChanged(nameof(EndTime));

            }
        }

        public TimeSpan Lunch
        {
            get { return countdownModel.Lunch; }
            set
            {
                countdownModel.Lunch = value; 
                RaisePropertyChanged(nameof(Lunch));

            }
        }

        public double WorkTime
        {
            get { return countdownModel.WorkTime; }
            set
            {
                countdownModel.WorkTime = value;
                RaisePropertyChanged(nameof(WorkTime));
                //if (value >= 6 && !LunchVoucher)
                //    LunchVoucher = true;
                //else if (value < 6 && LunchVoucher)
                //    LunchVoucher = false;
            }
        }

        public TimeSpan WorkTimeSpan
        {
            get
            {
                return TimeSpan.FromHours(WorkTime);
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

            SelectedDate = DateTime.Now;
        }

        private void StartCountdown()
        {
            LockInterface();


            ClearForm();
            PrepareData();
        }

        private void ClearForm()
        {
            Start = DateTime.Today;
            EndTime = DateTime.Today;
            Lunch = TimeSpan.FromMinutes(30);
        }

        private void PrepareData()
        {
            //var settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            //var getter = new Watt(settingsService.UserName, settingsService.Password);
            //var reportHtml = getter.LoginAndGetReportHtml();
            var reportHtml = File.ReadAllText("D:\\WATT2.htm");
            var parser = new WattParser(reportHtml);
            var entries = parser.ParseEntries(SelectedDate);
            if (entries.First().Type != EntryType.Prichod)
                throw new Exception("First entry is not of type 'Prichod'");
            Start = entries.First().EntryTime;
            Lunch = GetTimeToSubtract(entries);
            EndTime = entries.Last().EntryTime;
            var timeToLeave = Start + WorkTimeSpan + Lunch;
            
            if (entries.Count > 3 && entries.Count % 2 == 0 && entries.Last().Type == EntryType.Odchod)
            {
                Countdown = timeToLeave - EndTime;
                timer.Stop();
                
            }
            else
            {
                Countdown = timeToLeave - DateTime.Now;
                timer.Start();
            }

            SetCountdownLabel();
        }

        private TimeSpan GetTimeToSubtract(List<WattEntry> entries)
        {
            TimeSpan sumToSubtract = new TimeSpan();
            for (int i = 1; i < entries.Count-1; i+=2)
            {
                var startEntry = entries[i];
                var endEntry = entries[i+1];

                if(startEntry.Type == EntryType.Prichod || endEntry.Type == EntryType.Odchod)
                    throw new NotSupportedException("Your WATT report is not correct");

                var toSubtract = endEntry.EntryTime - startEntry.EntryTime;
                sumToSubtract += toSubtract;
            }

            if (sumToSubtract < HalfHour)
                sumToSubtract = HalfHour;

            return sumToSubtract;
        }

        private void StopCountdown()
        {
            timer.Stop();
            UnlockInterface();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Countdown = Countdown.Subtract(timer.Interval);

            //if (Countdown.TotalSeconds < 0)
            //{
            //    timer.Stop();
            //    CountdownLabel = Properties.Resources.CountdownFinished;
            //}
            //else
            SetCountdownLabel();
        }


        private void SetCountdownLabel()
        {
            CountdownLabel = (Countdown < TimeSpan.Zero ? "-" : "+") +  Countdown.ToString(@"hh\:mm\:ss");
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
