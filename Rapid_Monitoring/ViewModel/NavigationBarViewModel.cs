using Rapid_Monitoring.Infrastructure.Commands;
using Rapid_Monitoring.Model;
using System.Windows.Input;


namespace Rapid_Monitoring.ViewModel
{
    public class NavigationBarViewModel
    {

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
            INavigationService connectionNavigationService)
        {
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
    }
}
