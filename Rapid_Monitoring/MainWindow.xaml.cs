using Lab_Stenter_Dryer.Components;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.ViewModel;
using Lab_Stenter_Dryer.Services;
using System.Windows;

namespace Lab_Stenter_Dryer
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