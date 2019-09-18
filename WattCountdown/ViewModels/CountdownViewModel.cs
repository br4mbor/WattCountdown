using Abb.Cz.Apps.WattCountdown.Helpers;
using Abb.Cz.Apps.WattCountdown.Interfaces;
using Abb.Cz.Apps.WattCountdown.Models;
using Abb.Cz.Apps.WattCountdown.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

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
        public DateTime SelectedDate
        {
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

        private string _timeToLeaveLabel;
        public string TimeToLeaveLabel
        {
            get
            {
                return _timeToLeaveLabel;
            }
            set
            {
                _timeToLeaveLabel = value;
                RaisePropertyChanged(nameof(TimeToLeaveLabel));
            }
        }

        private DateTime _timeToLeave;
        public DateTime TimeToLeave
        {
            get
            {
                return _timeToLeave;
            }
            set
            {
                _timeToLeave = value;
                RaisePropertyChanged(nameof(TimeToLeave));
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

        private string _errorMessage;

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

        private System.Windows.Visibility _errorVisible;
        public System.Windows.Visibility ErrorVisible
        {
            get { return _errorVisible; }
            private set
            {
                _errorVisible = value;
                RaisePropertyChanged(nameof(ErrorVisible));
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

        public bool AutoModeEnabled { get; set; }
        public bool ManualModeEnabled { get; set; }

        #endregion

        #region Commands
        public ICommand StartCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        public ICommand DateChangedCommand { get; private set; }

        public ICommand TodayCommand { get; private set; }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion

        public CountdownViewModel()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;

            StartCommand = new RelayCommand(StartCountdown);
            StopCommand = new RelayCommand(StopCountdown);
            DateChangedCommand = new RelayCommand(DateChanged);
            TodayCommand = new RelayCommand(ChangeToToday);

            SelectedDate = DateTime.Now;

            //StartCountdown();
        }

        private void ChangeToToday()
        {
            SelectedDate = DateTime.Today;
            DateChanged();
        }

        private void DateChanged()
        {
            if (!AutoModeEnabled)
                return;

            StopCountdown();
            StartCountdown();
        }

    private void StartCountdown()
        {
            ClearForm();
            try
            {
                PrepareData();
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }

            SetLabels();
            LockInterface();
        }

        private void ClearForm()
        {
            Start = DateTime.Today;
            EndTime = DateTime.Today;
            Lunch = TimeSpan.FromMinutes(30);
            TimeToLeave = DateTime.Today;
            TimeToLeaveLabel = "";
            CountdownLabel = "";
            ErrorMessage = "";
        }

        private void PrepareData()
        {
            var settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            var getter = new Watt(settingsService.UserName, settingsService.Password);
            var reportHtml = getter.LoginAndGetReportHtml();
            //var parser = new WattParser(File.ReadAllText("D:\\WATT.htm"));
            var parser = new WattParser(reportHtml);
            var entries = parser.ParseEntries(SelectedDate);
            if (!entries.Any())
                throw new Exception("There are no entries for selected date.");
            if (entries.First().Type != EntryType.Prichod)
                throw new Exception("First entry is not of type 'Prichod'");
            Start = entries.First().EntryTime;
            Lunch = GetTimeToSubtract(entries);

            if (entries.Last().Type == EntryType.Odchod)
                EndTime = entries.Last().EntryTime;

            TimeToLeave = Start + WorkTimeSpan + Lunch;

            if (entries.Count > 3 && entries.Count % 2 == 0 && entries.Last().Type == EntryType.Odchod)
            {
                Countdown = TimeToLeave - EndTime;
                timer.Stop();

            }
            else
            {
                Countdown = TimeToLeave - DateTime.Now;
                timer.Start();
            }

            
        }

        private TimeSpan GetTimeToSubtract(List<WattEntry> entries)
        {
            TimeSpan sumToSubtract = new TimeSpan();
            for (int i = 1; i < entries.Count - 1; i += 2)
            {
                var startEntry = entries[i];
                var endEntry = entries[i + 1];

                if (startEntry.Type == EntryType.Prichod || endEntry.Type == EntryType.Odchod)
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
            SetLabels();
        }


        private void SetLabels()
        {
            CountdownLabel = (Countdown < TimeSpan.Zero ? "(over) " : "(missing) ") + Countdown.ToString(@"hh\:mm\:ss");
            TimeToLeaveLabel = TimeToLeave.ToShortTimeString();
        }

        private void SetError(string error)
        {
            ErrorMessage = error;
        }

        private void LockInterface()
        {
            StartEnabled = false;
            WorkTimeEnabled = false;

            if (string.IsNullOrEmpty(ErrorMessage))
                CountdownVisible = System.Windows.Visibility.Visible;
            else
                ErrorVisible = System.Windows.Visibility.Visible;

            StartButtonEnabled = false;
            StopButtonEnabled = true;
        }

        private void UnlockInterface()
        {
            StartEnabled = true;
            WorkTimeEnabled = true;
            CountdownVisible = System.Windows.Visibility.Hidden;
            ErrorVisible = System.Windows.Visibility.Hidden;
            StartButtonEnabled = true;
            StopButtonEnabled = false;

        }
    }
}
