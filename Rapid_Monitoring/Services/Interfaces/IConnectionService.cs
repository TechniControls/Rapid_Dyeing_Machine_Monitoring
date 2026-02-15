using System;
using System.Collections.Generic;
using System.Text;

namespace Rapid_Monitoring.Services.Interfaces
{
    public interface IConnectionService
    {
        bool ConnectPlc(string cpuType, string ipAddress, string rack, string slot);
        bool DisconnectPlc();
    }
}
