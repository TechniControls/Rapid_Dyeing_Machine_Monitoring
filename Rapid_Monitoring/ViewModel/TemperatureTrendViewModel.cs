using System.Collections.ObjectModel;
using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Services;
using ScottPlot;
using System.Windows;
using Lab_Stenter_Dryer.Store;
using Lab_Stenter_Dryer.Model;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class TemperatureTrendViewModel : ViewModelBase
    {
        private readonly TemperatureStore _temperatureStore;

        // Exponer histórico del Store
        public IReadOnlyList<TemperaturePoint> HistoricalData => _temperatureStore.HistoricalData;

        // Evento para la vista (tiempo, PV, SP)
        public event Action<double, float, float, float>? NewSample;

        public TemperatureTrendViewModel(TemperatureStore temperatureStore)
        {
            _temperatureStore = temperatureStore;

            // Reenviar evento del Store
            _temperatureStore.NewSample += (t, pv1, pv2, sp) => NewSample?.Invoke(t, pv1, pv2, sp);
        }
    }
}
