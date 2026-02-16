using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using Rapid_Monitoring.Infrastructure.Base;
using Rapid_Monitoring.Services.Interfaces;
using S7.Net;

namespace Rapid_Monitoring.Services
{
    public static class ConnectionService
    {
        private static Plc _plcStation;

        #region PLC Addresses
        private const string _tempAddress = "DB1.DBD0";
        private const string _timeAddress = "DB1.DBD1";
        private const string _speedAddress = "DB1.DBD2";
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
        public static void WriteRecipe(string temperature, string time, string speed)
        {
            _plcStation.Write(_tempAddress, float.Parse(temperature));
            _plcStation.Write(_timeAddress, float.Parse(time));
            _plcStation.Write(_speedAddress, float.Parse(speed));
        }
        #endregion

    }

}
