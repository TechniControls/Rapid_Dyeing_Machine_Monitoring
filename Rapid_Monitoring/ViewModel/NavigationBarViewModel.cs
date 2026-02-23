using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Store;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;


namespace Lab_Stenter_Dryer.ViewModel
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly ConnectionStore _connectionStore;
        private bool IsConnected => _connectionStore.IsConnected;

        private readonly INavigationService _temperatureTrendNavigationService;
        private readonly INavigationService _processControlNavigationService;
        private readonly INavigationService _homeNavigationService;
        private readonly INavigationService _connectionNavigationService;
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateProcessControlCommand { get; }
        public ICommand NavigateTemperatureTrendCommand { get; }
        public ICommand OpenConnectionCommand {  get; }

        public NavigationBarViewModel(
            INavigationService temperatureTrendNavigationService,
            INavigationService processControlNavigationService,
            INavigationService homeNavigationService,
            INavigationService connectionNavigationService,
            ConnectionStore connectionStore)
        {
            _connectionStore = connectionStore;

            _connectionStore.PropertyChanged += OnConnectionStoreChanged;

            _temperatureTrendNavigationService = temperatureTrendNavigationService;
            NavigateTemperatureTrendCommand = new RelayCommand (
                _ => _temperatureTrendNavigationService.NavigateTo<TemperatureTrendViewModel>());

            _processControlNavigationService = processControlNavigationService;
            NavigateProcessControlCommand = new RelayCommand(
                _ => _processControlNavigationService.NavigateTo<ProcessControlViewModel>());

            _homeNavigationService = homeNavigationService;
            NavigateHomeCommand = new RelayCommand(
                _ => _homeNavigationService.NavigateTo<HomeViewModel>());

            _connectionNavigationService = connectionNavigationService;
            OpenConnectionCommand = new RelayCommand(
                _ => _connectionNavigationService.OpenWindow<ConnectionViewModel>());

        }

        // Connection Status
        public string ConnectionStatus =>
            IsConnected ? "Connected" : "Disconnected";
        public Brush ColorConnectionStatus =>
            IsConnected ? Brushes.Green : Brushes.Orange;

        private void OnConnectionStoreChanged(object? sender, PropertyChangedEventArgs e)

        {
            if (e.PropertyName == nameof(_connectionStore.IsConnected))
            {
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ColorConnectionStatus));
            }
        }
    }
}
