using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Services;
using System.Windows.Media;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly RecipesModel _recipesModel;

        #region Relay Commands
        public RelayCommand LoadRecipeOneCommand { get; }
        public RelayCommand LoadRecipeTwoCommand { get; }
        public RelayCommand LoadRecipeThreeCommand { get; }
        public RelayCommand LoadRecipeFourCommand { get; }
        #endregion

        public HomeViewModel()
        {
            _recipesModel = new RecipesModel();

            // Init Commands for Load Recipes
            // Polyester Recipe
            LoadRecipeOneCommand = new RelayCommand(_ => ConnectionService.WriteRecipe
            (
                PolyesterTemperature,
                PolyesterProcessSpeed,
                PolyesterProcessTime
            ));
            // Powernet Recipe
            LoadRecipeTwoCommand = new RelayCommand(_ => ConnectionService.WriteRecipe
            (
                PowernetTemperature,
                PowernetProcessTime,
                PowernetProcessSpeed
            ));
            //// Blonda Recipe
            LoadRecipeThreeCommand = new RelayCommand(_ => ConnectionService.WriteRecipe
            (
                PowernetTemperature,
                PowernetProcessTime,
                PowernetProcessSpeed
            ));
            //// Decoration Recipe
            LoadRecipeFourCommand = new RelayCommand(_ => ConnectionService.WriteRecipe
            (
                PowernetTemperature,
                PowernetProcessTime,
                PowernetProcessSpeed
            ));

        }

        #region Recipe Properties
        //Polyester Recipe
        public string PolyesterTemperature => _recipesModel.PolyesterTemperature;
        public string PolyesterProcessTime => _recipesModel.PolyesterTime;
        public string PolyesterProcessSpeed => _recipesModel.PolyesterSpeed;

        //Powernet Recipe
        public string PowernetTemperature => _recipesModel.PowernetTemperature;
        public string PowernetProcessTime => _recipesModel.PowernetTime;
        public string PowernetProcessSpeed => _recipesModel.PowernetSpeed;

        //Blonda Recipe
        public string BlondaTemperature => _recipesModel.BlondaTemperature;
        public string BlondaProcessTime => _recipesModel.BlondaTime;
        public string BlondaProcessSpeed => _recipesModel.BlondaSpeed;

        //Decoration Recipe
        public string DecorationTemperature => _recipesModel.DecorationTemperature;
        public string DecorationProcessTime => _recipesModel.DecorationTime;
        public string DecorationProcessSpeed => _recipesModel.DecorationSpeed;
        #endregion

        #region Current Process Status Properties
        public string CurrentTemp
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }
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
        public string MachineStatus => 
            CurrentMachineStatus? "Running" : "Stopped";
        public Brush ColorMachineStatus =>
            CurrentMachineStatus ? Brushes.Green : Brushes.Orange;

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
    }
}
