namespace Lab_Stenter_Dryer.Model
{
    public class RecipesModel
    {
        //Polyester Recipe
        public string PolyesterTemperature { get; } = "90";
        public string PolyesterTime { get; } = "15";
        public string PolyesterSpeed{ get; } = "58";

        // Powernet Recipe
        public string PowernetTemperature { get; } = "90";
        public string PowernetTime { get; } = "10";
        public string PowernetSpeed { get; } = "25";

        // Blonda Recipe
        public string BlondaTemperature { get; } = "75";
        public string BlondaTime { get; } = "2";
        public string BlondaSpeed { get; } = "300";

        // Blonda Recipe
        public string DecorationTemperature { get; } = "75";
        public string DecorationTime { get; } = "2";
        public string DecorationSpeed { get; } = "300";
    }
}
