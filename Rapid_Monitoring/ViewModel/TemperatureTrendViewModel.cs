using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Store;
using Lab_Stenter_Dryer.Model;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class TemperatureTrendViewModel : ViewModelBase
    {
        private readonly TemperatureStore _temperatureStore;

        // Expose HistoricalData from the Store
        public IReadOnlyList<TemperaturePoint> HistoricalData => _temperatureStore.HistoricalData;

        // Event to update view (Time, PT100-1, PT100-2, SetPoint)
        public event Action<double, float, float, float>? NewSample;

        public TemperatureTrendViewModel(TemperatureStore temperatureStore)
        {
            _temperatureStore = temperatureStore;

            // Reenviar evento del Store
            _temperatureStore.NewSample += (t, pv1, pv2, sp) => NewSample?.Invoke(t, pv1, pv2, sp);
        }
    }
}
