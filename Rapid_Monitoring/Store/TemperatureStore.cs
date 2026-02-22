using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Model;
using System.Security.Cryptography.X509Certificates;

namespace Lab_Stenter_Dryer.Store
{
    public class TemperatureStore : ViewModelBase
    {
        public List<TemperaturePoint> HistoricalData { get; } = new();
        public event Action<double, float, float, float>? NewSample;
        public float FirstPt100
        {
            get => field;
            set
            {
                if (field == value)
                    return;

                field = value;
                OnPropertyChanged();
            }
        }
        public float SecondPt100
        {
            get => field;
            set
            {
                if (field == value)
                    return;

                field = value;
                OnPropertyChanged();
            }
        }

        public void AddSample(float processFirstPt100, float processSecondPt100, float setPoint)
        {
            double time = DateTime.Now.ToOADate();

            HistoricalData.Add(new TemperaturePoint
            {
                Time = time,
                ProcessFirstPt100 = processFirstPt100,
                ProcessSecondPt100 = processSecondPt100,
                SetPoint = setPoint
            });

            NewSample?.Invoke(time, processFirstPt100, processSecondPt100, setPoint);

            FirstPt100 = processFirstPt100;
            SecondPt100 = processSecondPt100;

        }
    }
}
