using Rapid_Monitoring.Infrastructure.Base;
using Rapid_Monitoring.Infrastructure.Commands;
using Rapid_Monitoring.Model;
using Rapid_Monitoring.Services;
using Rapid_Monitoring.Store;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Net;
using System.Windows;

namespace Rapid_Monitoring.ViewModel
{
    public class ConnectionViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Instance Classes
        private readonly ConnectionSetupModel _connectionSetupModel;
        private readonly ConnectionStore _connectionStore;
        #endregion

        #region Relays Commands
        public RelayCommand OpenConnectionCommand { get; }
        public RelayCommand CloseConnectionCommand { get; }

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
            OpenConnectionCommand = new RelayCommand(_ => OpenConnection(), _ => !IsConnected);
            CloseConnectionCommand = new RelayCommand(_ => CloseConnection(), _ => IsConnected);
        }

        #region Properties
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
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ConnectionStatusColor));
                OpenConnectionCommand.RaiseCanExecuteChanged();
                CloseConnectionCommand.RaiseCanExecuteChanged();
            }
        }
        // Connection Status Indicator
        public string ConnectionStatus => IsConnected ? "Connected" : "Disconnected";
        public Brush ConnectionStatusColor => IsConnected ? Brushes.Green : Brushes.Orange;

        // IDataErrorInfo Implementation
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(PlcIpAddress))
                {
                    if (string.IsNullOrEmpty(PlcIpAddress))
                        return MessageBox.Show("IP Address cannot be empty.").ToString();

                    if (!IPAddress.TryParse(PlcIpAddress, out _))
                        return MessageBox.Show("Invalid IP Address format.").ToString();
                }
                return null;
            }
        }
        #endregion

        #region Methods
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
                IsConnected = false;
        }
        #endregion
    }
}