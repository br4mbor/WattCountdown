using Abb.Cz.Apps.WattCountdown.Interfaces;
using Abb.Cz.Apps.WattCountdown.Models;
using Abb.Cz.Apps.WattCountdown.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Abb.Cz.Apps.WattCountdown.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields
        private LoginModel loginModel = new LoginModel();
        #endregion

        #region Properties
        public string UserName
        {
            get { return loginModel.UserName; }
            set { loginModel.UserName = value; }
        }

        public string Password
        {
            get { return loginModel.Password; }
            set { loginModel.Password = value; }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get; private set; } 
        #endregion

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<string>(Login);

            var settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();

            if (settingsService == null) return;

            UserName = settingsService.UserName;
            Password = settingsService.Password;
        }

        private void Login(string password)
        {
            Password = password;
            SaveLoginInfo();

            SimpleIoc.Default.GetInstance<NavigationService>().Navigate(new CountdownView());
        }

        private void SaveLoginInfo()
        {
            var settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();

            settingsService.UserName = UserName;
            settingsService.Password = Password;
            settingsService.Save();
        }
    }
}
