using Rapid_Monitoring.Model;
using Rapid_Monitoring.Store;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Rapid_Monitoring.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        #region Instance Classes
        private readonly ConnectionSetupModel _connectionSetupModel;
        private readonly ConnectionStore _connectionStore;
        #endregion

        public ConnectionViewModel(ConnectionStore connectionStore)
        {
            _connectionSetupModel = new ConnectionSetupModel();
            _connectionStore = connectionStore;
        }

        public ObservableCollection<string> CpuTypes => _connectionSetupModel.CpuType;
        public ObservableCollection<int> Racks => _connectionSetupModel.Rack;
        public ObservableCollection<int> Slots => _connectionSetupModel.Slot;

        public string SelectedCpuType
        {
            get => _connectionStore.SelectedCpuType;
            set
            {
                _connectionStore.SelectedCpuType = value;
                OnPropertyChanged();
                Debug.WriteLine($"Selected CPU Type: {value}");
            }
        }

        public string PlcIpAddress
        {
            get => _connectionStore.IpAddress;
            set
            {
                _connectionStore.IpAddress = value;
                OnPropertyChanged();
                Debug.WriteLine(value);
            }
        }

        public string SelectedRack
        {
            get => _connectionStore.SelectedRack;
            set
            {
                _connectionStore.SelectedRack = value;
                OnPropertyChanged();
                Debug.WriteLine($"Selected Rack: {value}");
            }
        }

        public string SelectedSlot
        {
            get => _connectionStore.SelectedSlot;
            set
            {
                _connectionStore.SelectedSlot= value;
                OnPropertyChanged();
                Debug.WriteLine($"Selected Slot: {value}");
            }
        }
    }
}