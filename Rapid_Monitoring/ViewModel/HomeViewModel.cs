using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Services;
using Lab_Stenter_Dryer.Store;
using S7.Net;
using System.ComponentModel;
using System.Windows.Media;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly RecipesModel _recipesModel;
        private readonly TemperaturePoint _temperaturePoint;
        private readonly ConnectionStore _connectionStore;
        private readonly ConnectionService _connectionService;
        private readonly TemperatureStore _temperatureStore;

        private bool IsConnected => _connectionStore.IsConnected;

        #region Relay Commands
        public RelayCommand LoadRecipeOneCommand { get; }
        public RelayCommand LoadRecipeTwoCommand { get; }
        public RelayCommand LoadRecipeThreeCommand { get; }
        public RelayCommand LoadRecipeFourCommand { get; }
        #endregion

        public HomeViewModel(ConnectionStore connectionStore,
            ConnectionService connectionService,
            TemperatureStore temperatureStore)
        {
            _recipesModel = new RecipesModel();
            _temperaturePoint = new TemperaturePoint();
            _connectionStore = connectionStore;
            _connectionService = connectionService;
            _temperatureStore = temperatureStore;
            _connectionStore.PropertyChanged += OnConnectionStoreChanged;
            _temperatureStore.PropertyChanged += OnTemperatureStoreChanged;


            // Init Commands for Load Recipes
            // Polyester Recipe
            LoadRecipeOneCommand = new RelayCommand(_ => _connectionService.WriteRecipe
            (
                PolyesterExtractorSpeed,
                PolyesterFanSpeed,
                PolyesterTemperature,
                PolyesterProcessTime
            ), _ => IsConnected);
            // Powernet Recipe
            LoadRecipeTwoCommand = new RelayCommand(_ => _connectionService.WriteRecipe
            (
                PowernetExtractorSpeed,
                PowernetFanSpeed,
                PowernetTemperature,
                PowernetProcessTime
            ), _ => IsConnected);
            // Blonda Recipe
            LoadRecipeThreeCommand = new RelayCommand(_ => _connectionService.WriteRecipe
            (
                BlondaExtractorSpeed,
                BlondaFanSpeed,
                BlondaTemperature,
                BlondaProcessTime
            ), _ => IsConnected);
            // Decoration Recipe
            LoadRecipeFourCommand = new RelayCommand(_ => _connectionService.WriteRecipe
            (
                DecorationExtractorSpeed,
                DecorationFanSpeed,
                DecorationTemperature,
                DecorationProcessTime
            ), _ => IsConnected);
        }

        #region Recipe Properties
        //Polyester Recipe
        public float PolyesterTemperature => _recipesModel.PolyesterTemperature;
        public float PolyesterProcessTime => _recipesModel.PolyesterTime;
        public float PolyesterFanSpeed => _recipesModel.PolyesterFanSpeed;
        public float PolyesterExtractorSpeed => _recipesModel.PolyesterExtractorSpeed;

        //Powernet Recipe
        public float PowernetTemperature => _recipesModel.PowernetTemperature;
        public float PowernetProcessTime => _recipesModel.PowernetTime;
        public float PowernetFanSpeed => _recipesModel.PowernetFanSpeed;
        public float PowernetExtractorSpeed => _recipesModel.PowernetExtractorSpeed;

        //Blonda Recipe
        public float BlondaTemperature => _recipesModel.BlondaTemperature;
        public float BlondaProcessTime => _recipesModel.BlondaTime;
        public float BlondaFanSpeed => _recipesModel.BlondaFanSpeed;
        public float BlondaExtractorSpeed => _recipesModel.BlondaExtractorSpeed;

        //Decoration Recipe
        public float DecorationTemperature => _recipesModel.DecorationTemperature;
        public float DecorationProcessTime => _recipesModel.DecorationTime;
        public float DecorationFanSpeed => _recipesModel.DecorationFanSpeed;
        public float DecorationExtractorSpeed => _recipesModel.DecorationExtractorSpeed;
        #endregion

        private void OnTemperatureStoreChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_temperatureStore.FirstPt100))
            {
                OnPropertyChanged(nameof(ValueFirstPT100));
                OnPropertyChanged(nameof(TemperatureDelta));

            }
            if (e.PropertyName == nameof(_temperatureStore.SecondPt100))
            {
                OnPropertyChanged(nameof(ValueSecondPT100));
                OnPropertyChanged(nameof(TemperatureDelta));
            }
        }

        #region Current Process Status Properties
        public float ValueFirstPT100 => _temperatureStore.FirstPt100;

        public float ValueSecondPT100 => _temperatureStore.SecondPt100;

        public float TemperatureDelta => ValueFirstPT100 - ValueSecondPT100;
        public bool CurrentMachineStatus
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(MachineStatus));
                OnPropertyChanged(nameof(ColorMachineStatus));
            }
        }
        public bool CurrentAlarmsStatus
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged(nameof(AlarmsStatus));
            }
        }

        // Connection Status
        public string ConnectionStatus =>
            IsConnected ? "Connected" : "Disconnected";
        public Brush ColorConnectionStatus =>
            IsConnected ? Brushes.Green : Brushes.Orange;

        // Machine Status
        public string MachineStatus =>
            CurrentMachineStatus ? "Running" : "Stopped";
        public Brush ColorMachineStatus =>
            CurrentMachineStatus ? Brushes.Green : Brushes.Orange;

        // Alarms Status
        public string AlarmsStatus =>
            CurrentAlarmsStatus ? "Active" : "Inactive";
        public Brush ColorAlarmsStatus =>
            CurrentAlarmsStatus ? Brushes.Red : Brushes.Green;
        public string CurrentRecipe
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void OnConnectionStoreChanged (object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_connectionStore.IsConnected))
            {
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ColorConnectionStatus));

                // Commands
                LoadRecipeOneCommand.RaiseCanExecuteChanged();
                LoadRecipeTwoCommand.RaiseCanExecuteChanged();
                LoadRecipeThreeCommand.RaiseCanExecuteChanged();
                LoadRecipeFourCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
