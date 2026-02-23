using Lab_Stenter_Dryer.Infrastructure.Base;
using Lab_Stenter_Dryer.Services.Interfaces;
using Lab_Stenter_Dryer.Store;
using S7.Net;
using System.Diagnostics;
using System.Windows;

namespace Lab_Stenter_Dryer.Services
{
    public class ConnectionService
    {
        private readonly TemperatureStore _temperatureStore;
        private readonly ConnectionStore _connectionStore;
        private Plc? _plcStation;
        private CancellationTokenSource? _cts;

        public bool EnableCustomRecipes = false;

        public ConnectionService(TemperatureStore temperatureStore, ConnectionStore connectionStore)
        {
            _temperatureStore = temperatureStore;
            _connectionStore = connectionStore;
        }

        #region PLC Addresses
        // Recipe Write Addresses
        private const string _extractorSpeedAddress = "DB1.DBD0"; // Address for extractor speed DB1 4.0
        private const string _fanSpeedAddress = "DB1.DBD4"; // Address for fan speed DB1 4.4
        private const string _temperatureAddress = "DB1.DBD8"; // Address for temperature DB1 8.0 
        private const string _processTimeAddress = "DB1.DBD12"; // Address for process time DB1 12.0

        // Read Addresses
        private const int _readFirstPt100 = 16; // "DB1.DBD16";  Read temperature from PT100-1
        private const int _readSecondPt100 = 20; // "DB1.DBD20";  Read temperature from PT100-2

        // Read Temperature
        private const int _dbNumber = 1;
        private const int _varCount = 1;

        #endregion

        private float ReadReal(int startByte)
        {
            return (float)_plcStation.Read(DataType.DataBlock, _dbNumber, startByte, VarType.Real, _varCount);
        }

        #region PLC Connection Methods
        public bool ConnectPlc(string cpuType, string ipAddress, string rack, string slot)
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
                _connectionStore.IsConnected = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error connecting to PLC: {e.Message}");
                return false;
            }
        }

        public bool DisconnectPlc()
        {
            try
            {
                // Close Connection
                _plcStation.Close();
                _plcStation = null;
                _connectionStore.IsConnected = false;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error disconnecting from PLC: {e.Message}");
                return false;
            }
        }
        #endregion

        #region Method for Write Recipes
        // Write predifined recipe
        public void WriteRecipe(float extractorSpeed, float fanSpeed, float setPointTemperature, float processTime)
        {
            try
            {
                _plcStation.Write(_extractorSpeedAddress, extractorSpeed); // Write extractor speed value
                _plcStation.Write(_fanSpeedAddress, fanSpeed); // Write fan speed value
                _plcStation.Write(_temperatureAddress, setPointTemperature); // Write temperature value
                _plcStation.Write(_processTimeAddress, processTime); // Write process time value
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error {e.Message}");
            }
        }
        #endregion

        #region Methods for Write Custom Recipe
        // Write each value for custom recipe
        // Write Temperature
        public void WriteCustomTemperature(float setPointTemperature)
        {
            _plcStation.Write(_temperatureAddress, setPointTemperature);
        }
        // Write Fan Speed
        public void WriteCustomFanSpeed(float fanSpeed)
        {
            _plcStation.Write(_fanSpeedAddress, fanSpeed);
        }
        // Write Extractor Speed
        public void WriteCustomExtractorSpeed(float extractorSpeed)
        {
            _plcStation.Write(_extractorSpeedAddress, extractorSpeed);
        }
        // Write Duration Time
        public void WriteCustomDurationTime(float processTime)
        {
            _plcStation.Write(_processTimeAddress, processTime);
        }
        #endregion

        #region Methods for Stop, Start, Reset, Pause Process
        // Start Process
        public void StartProcess()
        {
            // Example: Write a specific value to start the process
            _plcStation.Write("DB1.DBX0.0", true); // Assuming DB1.DBX0.0 is the start bit
        }
        // Stop Process
        public void StopProcess()
        {
            // Example: Write a specific value to start the process
            //_plcStation.Write("DB1.DBX0.0", true); // Assuming DB1.DBX0.0 is the start bit
            _cts.Cancel(); // Stop reading temperature
        }
        // Reset Process
        public void ResetProcess()
        {
            // Example: Write a specific value to start the process
            _plcStation.Write("DB1.DBX0.0", true); // Assuming DB1.DBX0.0 is the start bit
        }
        // Pause Process
        public void PauseProcess()
        {
            // Example: Write a specific value to start the process
            _plcStation.Write("DB1.DBX0.0", true); // Assuming DB1.DBX0.0 is the start bit
        }

        #endregion

        #region Read Current Process, temperature, time, speed

        public void ReadTemperature(float customSetPoint, float recipeSetPoint)
        {
            _cts = new CancellationTokenSource();
            float setPoint;

            Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        float rawValuePt1 = ReadReal(_readFirstPt100);
                        float rawValuePt2 = ReadReal(_readSecondPt100);

                        Debug.WriteLine($"pt100-1 {rawValuePt1}");
                        Debug.WriteLine($"pt100-2 {rawValuePt2}");

                        if (EnableCustomRecipes)
                        {
                            setPoint = customSetPoint;
                        }
                        else
                        {
                            setPoint = recipeSetPoint;
                        }
                        _temperatureStore.AddSample(rawValuePt1, rawValuePt2, setPoint);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error reading temperature: {ex.Message}");
                    }

                    await Task.Delay(1000, _cts.Token); // Read every second
                }
            }, _cts.Token);
        }


        #endregion

        #region Enable Custom Recipes

        public void ToggleEnableCustomRecipes()
        {
            EnableCustomRecipes = !EnableCustomRecipes;
        }
    }


    #endregion
}
