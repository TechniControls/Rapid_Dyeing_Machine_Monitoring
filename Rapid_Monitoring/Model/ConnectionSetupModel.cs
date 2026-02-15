using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rapid_Monitoring.Model
{
    public class ConnectionSetupModel
    {
        public ObservableCollection<string> CpuType = new ObservableCollection<string>
        {
            "S71200",
            "S71500",
            "LogoOBA8"
        };

        public ObservableCollection<int> Rack = new ObservableCollection<int>
        {
            0,1, 2, 3, 4, 5, 6, 7
        };

        public ObservableCollection<int> Slot = new ObservableCollection<int>
        {
            0,1, 2, 3, 4, 5, 6, 7
        };
    }
}
