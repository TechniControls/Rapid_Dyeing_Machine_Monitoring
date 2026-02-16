using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Rapid_Monitoring.Infrastructure.Base;
using S7.Net.Types;
using ScottPlot;

namespace Rapid_Monitoring.ViewModel
{
    public class TemperatureTrendViewModel : ViewModelBase
    {

        public ObservableCollection<DataPoint> Data
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged();
            }
        }

        public void LoadData()
        {
            List<int> datas = new List<int> { 1, 2, 3, 4 };
            Data = new ObservableCollection<DataPoint>(datas);
        }
    }
}
