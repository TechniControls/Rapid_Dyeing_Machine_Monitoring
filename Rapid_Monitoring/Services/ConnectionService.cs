using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Rapid_Monitoring.Infrastructure.Base;
using Rapid_Monitoring.Services.Interfaces;
using S7.Net;

namespace Rapid_Monitoring.Services
{
    public static class ConnectionService 
    {
        private static Plc _plc;

        public static bool ConnectPlc(string cpuType, string ipAddress, string rack, string slot)
        {
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
                _plc = new Plc(selectedCpuType, ipAddress, rackId, slotId);

                // Open Connection
                _plc.Open();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error connecting to PLC: {e.Message}");
                return false;
            }
        }

        public static bool DisconnectPlc()
        {
            try
            {
                // Close Connection
                _plc.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error disconnecting from PLC: {e.Message}");
                return false;
            }
        }

    }

}
