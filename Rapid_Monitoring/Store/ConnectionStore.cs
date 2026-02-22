using Lab_Stenter_Dryer.Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_Stenter_Dryer.Store
{
    public class ConnectionStore : ViewModelBase
    {
        public string SelectedCpuType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string SelectedRack { get; set; } = string.Empty;
        public string SelectedSlot { get; set; } = string.Empty;
        public bool IsConnected
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
    }
}
