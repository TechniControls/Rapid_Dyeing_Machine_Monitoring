using System.Collections.ObjectModel;
using Lab_Stenter_Dryer.Infrastructure.Base;
using ScottPlot;

namespace Lab_Stenter_Dryer.ViewModel
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

       
    }
}
