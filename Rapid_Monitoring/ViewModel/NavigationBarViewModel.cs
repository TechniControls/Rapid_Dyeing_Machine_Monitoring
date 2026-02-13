using Rapid_Monitoring.Commands;
using Rapid_Monitoring.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


namespace Rapid_Monitoring.ViewModel
{
    public class NavigationBarViewModel
    {
        public string Title => "Encajes SA Dyeing Lab";

        private readonly INavigationService _temperatureTrendNavigationService;
        private readonly INavigationService _processControlNavigationService;
        private readonly INavigationService _homeNavigationService;
        private readonly INavigationService _connectionNavigationService;
        private readonly INavigationService _recipesNavigationService;
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateProcessControlCommand { get; }
        public ICommand NavigateRecipesCommand { get; }
        public ICommand NavigateTemperatureTrendCommand { get; }
        public ICommand OpenConnectionCommand {  get; }

        public NavigationBarViewModel(
            INavigationService temperatureTrendNavigationService,
            INavigationService processControlNavigationService,
            INavigationService homeNavigationService,
            INavigationService connectionNavigationService,
            INavigationService recipesNavigationService)
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

            _recipesNavigationService = recipesNavigationService;
            NavigateRecipesCommand = new RelayCommand(
                _ => _recipesNavigationService.NavigateTo<RecipesViewModel>());

        }
    }
}
