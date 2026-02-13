using Rapid_Monitoring.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rapid_Monitoring.ViewModel
{
    public class RecipesViewModel
    {
        private readonly RecipesModel _recipesModel;

        public RecipesViewModel()
        {
            _recipesModel = new RecipesModel();
        }

        //Polyester Recipe
        public string PolyesterRecipeTemperature=> _recipesModel.PolyesterTemperature;
        public string PolyesterProcessTime => _recipesModel.PolyesterTime;
        public string PolyesterProcessSpeed => _recipesModel.PolyesterSpeed;

        //Powernet Recipe
        public string PowernetRecipeTemperature => _recipesModel.PowernetTemperature;
        public string PowernetProcessTime => _recipesModel.PowernetTime;
        public string PowernetProcessSpeed => _recipesModel.PowernetSpeed;

        //Blonda Recipe
        public string BlondaRecipeTemperature => _recipesModel.BlondaTemperature;
        public string BlondaProcessTime => _recipesModel.BlondaTime;
        public string BlondaProcessSpeed => _recipesModel.BlondaSpeed;
    }
}
