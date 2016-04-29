using Abb.Cz.Apps.WattCountdown.Views;
using GalaSoft.MvvmLight.Ioc;
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
using System.Windows.Navigation;
using Abb.Cz.Apps.WattCountdown.Interfaces;
using Abb.Cz.Apps.WattCountdown.Services;

namespace Abb.Cz.Apps.WattCountdown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.Register<ISettingsService, LocalSettingsService>();
            SimpleIoc.Default.Register(() => MainFrame.NavigationService);
            //SimpleIoc.Default.GetInstance<NavigationService>().Navigate(new LoginView());
            SimpleIoc.Default.GetInstance<NavigationService>().Navigate(new CountdownView());
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.Unregister<NavigationService>();
        }
    }
}
