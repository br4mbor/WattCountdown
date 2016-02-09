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
        private LoginModel loginModel = new LoginModel();

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

        //public string ProxyUserName
        //{
        //    get { return loginModel.ProxyUserName; }
        //    set { loginModel.ProxyUserName = value; }
        //}

        //public string ProxyPassword
        //{
        //    get { return loginModel.ProxyPassword; }
        //    set { loginModel.ProxyPassword = value; }
        //}

        //public string ProxyAddress
        //{
        //    get { return loginModel.ProxyAddress; }
        //    set { loginModel.ProxyAddress = value; }
        //}

        //public int ProxyPort
        //{
        //    get { return loginModel.ProxyPort; }
        //    set { loginModel.ProxyPort = value; }
        //}

        //public bool UseProxy
        //{
        //    get { return loginModel.UseProxy; }
        //    set { loginModel.UseProxy = value; }
        //}

        public RelayCommand<string> LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<string>(Login);
        }

        private void Login(string password)
        {
            Password = password;

            SimpleIoc.Default.GetInstance<NavigationService>().Navigate(new CountdownView());
        }
    }
}
