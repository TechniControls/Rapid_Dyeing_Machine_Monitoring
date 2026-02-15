using Rapid_Monitoring.Infrastructure.Base;
using Rapid_Monitoring.Infrastructure.Commands;
using Rapid_Monitoring.Model;
using Rapid_Monitoring.Services;
using Rapid_Monitoring.Store;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Rapid_Monitoring.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        #region Instance Classes
        private readonly ConnectionSetupModel _connectionSetupModel;
        private readonly ConnectionStore _connectionStore;
        #endregion

        #region Relays Commands
        public RelayCommand ExecuteOpenConnectionCmd{ get; }
        public RelayCommand ExecuteCloseConnectionCmd { get; }

        #endregion

        #region Observable Collections
        public ObservableCollection<string> CpuTypes => _connectionSetupModel.CpuType;
        public ObservableCollection<int> Racks => _connectionSetupModel.Rack;
        public ObservableCollection<int> Slots => _connectionSetupModel.Slot;
        #endregion

        public ConnectionViewModel(ConnectionStore connectionStore)
        {
            _connectionSetupModel = new ConnectionSetupModel();
            _connectionStore = connectionStore;

            // Commands
            ExecuteOpenConnectionCmd = new RelayCommand(_ => OpenConnection(), _ => !IsConnected);
            ExecuteCloseConnectionCmd = new RelayCommand(_ => CloseConnection(), _ => IsConnected);
        }


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
                _connectionStore.SelectedSlot = value;
                OnPropertyChanged();
                Debug.WriteLine($"Selected Slot: {value}");
            }
        }

        public bool IsConnected
        {
            get => _connectionStore.IsConnected;
            set
            {
                _connectionStore.IsConnected = value;
                OnPropertyChanged();
            }
        }

        private void OpenConnection()
        {
            bool connection = ConnectionService.ConnectPlc(SelectedCpuType, PlcIpAddress, SelectedRack, SelectedSlot);

            if (connection)
                IsConnected = true;
        }

        private void CloseConnection()
        {
            bool connection = ConnectionService.DisconnectPlc();

            if (connection)
                IsConnected= false;
        }
    }
}