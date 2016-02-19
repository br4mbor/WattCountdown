using Abb.Cz.Apps.WattCountdown.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Abb.Cz.Apps.WattCountdown.Views
{
    /// <summary>
    /// Interaction logic for CountdownView.xaml
    /// </summary>
    public partial class CountdownView : Page
    {
        public CountdownView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var model = SimpleIoc.Default.GetInstance<CountdownViewModel>();
            var msg = string.Format("Start: {0}\nEnd: {1}\nWorkTime: {2}", model.Start, model.End, model.WorkTime);
            MessageBox.Show(msg, "Data", MessageBoxButton.OK);
        }
    }
}
