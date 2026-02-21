namespace Lab_Stenter_Dryer.Services.Interfaces
{
    public interface IConnectionService
    {
        bool ConnectPlc(string cpuType, string ipAddress, string rack, string slot);
        bool DisconnectPlc();
    }
}
