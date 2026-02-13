using System;
using System.Collections.Generic;
using System.Text;

namespace Rapid_Monitoring.Model
{
    public class RecipesModel
    {
        //Polyester Recipe
        public string PolyesterTemperature { get; } = "130°C";
        public string PolyesterTime { get; } = "15 min";
        public string PolyesterSpeed{ get; } = "50 RPM";

        // Powernet Recipe
        public string PowernetTemperature { get; } = "90°C";
        public string PowernetTime { get; } = "10 min";
        public string PowernetSpeed { get; } = "25 RPM";

        // Blonda Recipe
        public string BlondaTemperature { get; } = "75°C";
        public string BlondaTime { get; } = "2 min";
        public string BlondaSpeed { get; } = "300 RPM";
    }
}
