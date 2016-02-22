using Abb.Cz.Apps.WattCountdown.Interfaces;
using Abb.Cz.Apps.WattCountdown.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Abb.Cz.Apps.WattCountdown.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<LoginViewModel>().LoginCommand.Execute(Password.Password);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Password.Password = SimpleIoc.Default.GetInstance<LoginViewModel>().Password;
            UserName.Focus();
        }
    }
}
