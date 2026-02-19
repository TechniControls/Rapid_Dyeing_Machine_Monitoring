using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rapid_Monitoring.Store
{
    public class TemperatureStore
    {
        public ObservableCollection<double> ProcessValues { get; } = new();
        public ObservableCollection<double> SetPointValues { get; } = new();

        public double CurrentValue { get; set; }

        public double ProcessValue { get; set; } 
        public double SetPointValue { get; set; }
    }
}
