using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Services.Interfaces;
using S7.Net;
using System.Diagnostics;
using System.Windows;

namespace Lab_Stenter_Dryer.Services
{
    public static class ConnectionService
    {
        private static Plc _plcStation;

        #region PLC Addresses
        private const string _tempAddress = "DB1.DBW2"; // e.g. address for temperature
        private const string _timeAddress = "DB1.DBW6"; // e.g. address for time
        private const string _speedAddress = "DB1.DBW4"; // e.g. address for speed
        #endregion

        public static bool ConnectPlc(string cpuType, string ipAddress, string rack, string slot)
        {
            if (string.IsNullOrEmpty(cpuType) || string.IsNullOrEmpty(ipAddress) || string.IsNullOrEmpty(rack) || string.IsNullOrEmpty(slot))
            {
                MessageBox.Show("Connection parameters cannot be null or empty.");
                return false;
            }
            try
            {
                CpuType selectedCpuType = cpuType switch
                {
                    "S71200" => CpuType.S71200,
                    "S71500" => CpuType.S71500,
                    "LogoOBA8" => CpuType.Logo0BA8,
                    _ => CpuType.S71200
                };
                Int16 rackId = Int16.Parse(rack);
                Int16 slotId = Int16.Parse(slot);

                // Create PLC Instance
                _plcStation = new Plc(selectedCpuType, ipAddress, rackId, slotId);

                // Open Connection
                _plcStation.Open();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error connecting to PLC: {e.Message}");
                return false;
            }
        }

        public static bool DisconnectPlc()
        {
            try
            {
                // Close Connection
                _plcStation.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error disconnecting from PLC: {e.Message}");
                return false;
            }
        }

        #region Methods for Write Recipes
        // Polyester Recipe
        public static void WriteRecipe(string temperature, string speed, string time)
        {
            _plcStation.Write(_tempAddress, ushort.Parse(temperature));
            _plcStation.Write(_speedAddress, ushort.Parse(speed));
            _plcStation.Write(_timeAddress, ushort.Parse(time));
        }
        #endregion

    }

}
