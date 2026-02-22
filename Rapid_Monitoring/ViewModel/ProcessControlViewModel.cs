using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Services;
using Lab_Stenter_Dryer.Store;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class ProcessControlViewModel : ViewModelBase
    {
        private readonly CustomRecipeStore _customRecipeStore;
        private readonly ConnectionService _connectionService;
        private readonly ConnectionStore _connectionStore;
        private readonly RecipesModel _recipesModel;

        private bool IsConnected => _connectionStore.IsConnected;

        #region Relay Commands
        public RelayCommand SetTemperatureCommand { get; }
        public RelayCommand SetFanSpeedCommand { get; }
        public RelayCommand SetExtractorSpeedCommand { get; }
        public RelayCommand SetTimeCommand { get; }

        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand PauseCommand { get; }
        public RelayCommand AutoRecipeCommand { get; }


        #endregion

        public ProcessControlViewModel(CustomRecipeStore customRecipeStore,
            ConnectionService connectionService,
            ConnectionStore connectionStore)
        {
            _customRecipeStore = customRecipeStore;
            _connectionService = connectionService;
            _connectionStore = connectionStore;

            _recipesModel = new();

            // Update Properties
            _connectionStore.PropertyChanged += OnConnectionStoreChanged;

            // Initialize Commands
            SetTemperatureCommand = new RelayCommand(_ => _connectionService.WriteCustomTemperature(CustomTemperature), _ => _enableCustomRecipes);
            SetFanSpeedCommand = new RelayCommand(_ => _connectionService.WriteCustomFanSpeed(CustomFanSpeed), _ => _enableCustomRecipes);
            SetExtractorSpeedCommand = new RelayCommand(_ => _connectionService.WriteCustomExtractorSpeed(CustomExtractorSpeed), _ => _enableCustomRecipes);
            SetTimeCommand = new RelayCommand(_ => _connectionService.WriteCustomDurationTime(CustomDurationTime), _ => _enableCustomRecipes);

            StartCommand = new RelayCommand(_ => _connectionService.ReadTemperature(CustomTemperature,_recipesModel.BlondaTemperature));
            StopCommand = new RelayCommand(_ => _connectionService.StopProcess());
            ResetCommand = new RelayCommand(_ => _connectionService.ResetProcess(), _ => IsConnected);
            PauseCommand = new RelayCommand(_ => _connectionService.PauseProcess(), _ => IsConnected);

            AutoRecipeCommand = new RelayCommand(
                _ =>
                {
                    _connectionService.ToggleEnableCustomRecipes();
                    OnPropertyChanged(nameof(_enableCustomRecipes));
                    OnPropertyChanged(nameof(RecipesStatusBtn));
                    SetTemperatureCommand.RaiseCanExecuteChanged();
                    SetFanSpeedCommand.RaiseCanExecuteChanged();
                    SetExtractorSpeedCommand.RaiseCanExecuteChanged();
                    SetTimeCommand.RaiseCanExecuteChanged();
                }, _ => IsConnected);
        }

        private void OnConnectionStoreChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_connectionStore.IsConnected))
            {
                AutoRecipeCommand.RaiseCanExecuteChanged();
            }
        }

        #region Custom Recipes Properties

        private bool _enableCustomRecipes => _connectionService.EnableCustomRecipes;
        public string RecipesStatusBtn => _enableCustomRecipes ? "Manual" : "Auto";

        // Custom Temperature
        public float CustomTemperature
        {
            get => _customRecipeStore.CustomTemperature;
            set
            {
                _customRecipeStore.CustomTemperature = value;
                OnPropertyChanged();
            }
        }
        // Custom Fan Speed
        public float CustomFanSpeed
        {
            get => _customRecipeStore.CustomFanSpeed;
            set
            {
                _customRecipeStore.CustomFanSpeed = value;
                OnPropertyChanged();
            }
        }
        // Custom Extractor Speed
        public float CustomExtractorSpeed
        {
            get => _customRecipeStore.CustomExtractorSpeed;
            set
            {
                _customRecipeStore.CustomExtractorSpeed = value;
                OnPropertyChanged();
            }
        }

        // Custom Duration Time
        public float CustomDurationTime
        {
            get => _customRecipeStore.CustomDurationTime;
            set
            {
                _customRecipeStore.CustomDurationTime = value;
                OnPropertyChanged();
            }
        }

        #endregion


    }
}
