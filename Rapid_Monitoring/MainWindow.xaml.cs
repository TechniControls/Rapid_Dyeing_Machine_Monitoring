using Rapid_Monitoring.Components;
using Rapid_Monitoring.Model;
using Rapid_Monitoring.ViewModel;
using Rapid_Monitoring.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rapid_Monitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(
            MainViewModel viewModel,
            NavigationBar navigationBar,
            INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = viewModel;
            NavigationBarHost.Content = navigationBar;

            // Set the ContentControl for navigation
            (navigationService as NavigationService)?.SetContentControl(NavigationContent);
        }
    }
}