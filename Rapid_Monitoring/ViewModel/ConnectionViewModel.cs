using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Infrastructure.Commands;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Services;
using Lab_Stenter_Dryer.Store;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Net;
using System.Windows;
using System.Runtime.CompilerServices;

namespace Lab_Stenter_Dryer.ViewModel
{
    public class ConnectionViewModel : ViewModelBase, IDataErrorInfo
    {
        public static bool IsConnectedGlobal { get; set; }

        #region Instance Classes
        private readonly ConnectionSetupModel _connectionSetupModel;
        private readonly ConnectionStore _connectionStore;
        private readonly ConnectionService _connectionService;
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

        public ConnectionViewModel(ConnectionStore connectionStore, ConnectionService connectionService)
        {
            _connectionSetupModel = new ConnectionSetupModel();
            _connectionStore = connectionStore;
            _connectionService = connectionService;
            _connectionStore.PropertyChanged += OnConnectionStoreChanged;

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

        public bool IsConnected => _connectionStore.IsConnected;
        
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

        private void OnConnectionStoreChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_connectionStore.IsConnected))
            {
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConnectionStatus));
                OnPropertyChanged(nameof(ConnectionStatusColor));

                OpenConnectionCommand.RaiseCanExecuteChanged();
                CloseConnectionCommand.RaiseCanExecuteChanged();
            }
        }

        #region Methods
        private void OpenConnection()
        {


            bool connection = _connectionService.ConnectPlc(SelectedCpuType, PlcIpAddress, SelectedRack, SelectedSlot);

           
        }

        private void CloseConnection()
        {
            bool connection = _connectionService.DisconnectPlc();

            
        }
        #endregion
    }
}