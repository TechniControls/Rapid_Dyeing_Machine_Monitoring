using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Services;
using Lab_Stenter_Dryer.Store;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class ProcessControlViewModel : ViewModelBase
    {
        private readonly CustomRecipeStore _customRecipeStore;
        #region Relay Commands
        public RelayCommand SetTemperatureCommand { get; }
        public RelayCommand SetFanSpeedCommand { get; }
        public RelayCommand SetExtractorSpeedCommand { get; }
        public RelayCommand SetTimeCommand { get; }

        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand PauseCommand { get; }

        #endregion

        public ProcessControlViewModel(CustomRecipeStore customRecipeStore)
        {
            _customRecipeStore = customRecipeStore;
            // Initialize Commands
            SetTemperatureCommand = new RelayCommand(_ => ConnectionService.WriteCustomTemperature("j+"));
            SetFanSpeedCommand = new RelayCommand(_ => ConnectionService.WriteCustomFanSpeed("d"));
            SetExtractorSpeedCommand = new RelayCommand(_ => ConnectionService.WriteCustomExtractorSpeed("d"));
            SetTimeCommand = new RelayCommand(_ => ConnectionService.WriteCustomDurationTime("s"));

            StartCommand = new RelayCommand(_ => ConnectionService.StartProcess());
            StopCommand = new RelayCommand(_ => ConnectionService.StopProcess());
            ResetCommand = new RelayCommand(_ => ConnectionService.ResetProcess());
            PauseCommand = new RelayCommand(_ => ConnectionService.PauseProcess());
        }

        #region Custom Recipes Properties
        // Custom Temperature
        public string CustomTemperature
        {
            get => _customRecipeStore.CustomTemperature;
            set
            {
                _customRecipeStore.CustomTemperature = value;
                OnPropertyChanged();
            }
        }
        // Custom Fan Speed
        public string CustomFanSpeed
        {
            get => _customRecipeStore.CustomFanSpeed;
            set
            {
                _customRecipeStore.CustomFanSpeed = value;
                OnPropertyChanged();
            }
        }
        // Custom Extractor Speed
        public string CustomExtractorSpeed
        {
            get => _customRecipeStore.CustomExtractorSpeed;
            set
            {
                _customRecipeStore.CustomExtractorSpeed = value;
                OnPropertyChanged();
            }
        }

        // Custom Duration Time
        public string CustomDurationTime
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
