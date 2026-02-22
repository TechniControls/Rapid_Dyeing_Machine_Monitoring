namespace Lab_Stenter_Dryer.Model
{
    public class RecipesModel
    {
        //Polyester Recipe
        public float PolyesterTemperature { get; } = 98;
        public float PolyesterTime { get; } = 15;
        public float PolyesterFanSpeed{ get; } = 25.2f;
        public float PolyesterExtractorSpeed{ get; } = 12.6f;

        // Powernet Recipe
        public float PowernetTemperature { get; } = 105.6f;
        public float PowernetTime { get; } = 1f;
        public float PowernetFanSpeed { get; } = 35.3f;
        public float PowernetExtractorSpeed { get; } = 36.9f;

        // Blonda Recipe
        public float BlondaTemperature { get; } = 95.5f;
        public float BlondaTime { get; } = 2f;
        public float BlondaFanSpeed { get; } = 300.5f;
        public float BlondaExtractorSpeed { get; } = 250.4f;

        // Blonda Recipe
        public float DecorationTemperature { get; } = 56.7f;
        public float DecorationTime { get; } = 5f;
        public float DecorationFanSpeed { get; } = 14.6f;
        public float DecorationExtractorSpeed { get; } = 10.8f;
    }
}
