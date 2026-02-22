using Lab_Stenter_Dryer.ViewModel;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Lab_Stenter_Dryer.View
{
    public partial class TemperatureTrendView : UserControl
    {
        private TemperatureTrendViewModel? _vm;

        private SignalXY FirstPT100 = null!;
        private SignalXY SecondPT100 = null;
        private SignalXY _setPointSignal = null!;

        private double[] _xBuffer = null!;
        private double[] _processBufferFirstPT100 = null!;
        private double[] _processBufferSecondPT100 = null!;
        private double[] _setPointBuffer = null!;

        private int _writeIndex = 0;
        private bool _bufferFull = false;

        private const int BufferSize = 6000; // 10 minutos @ 100 ms

        private DispatcherTimer? _refreshTimer;

        public TemperatureTrendView()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
            Unloaded += OnUnloaded;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Desuscribirse del VM anterior
            if (_vm != null)
                _vm.NewSample -= OnNewSample;

            _vm = DataContext as TemperatureTrendViewModel;
            if (_vm == null)
                return;

            _refreshTimer?.Stop();
            ScatterPlot.Plot.Clear();

            // Inicializar buffers
            _xBuffer = new double[BufferSize];
            _processBufferFirstPT100 = new double[BufferSize];
            _processBufferSecondPT100 = new double[BufferSize];
            _setPointBuffer = new double[BufferSize];

            _writeIndex = 0;
            _bufferFull = false;

            // Restaurar histórico si existe
            if (_vm.HistoricalData.Count > 0)
            {
                int start = Math.Max(0, _vm.HistoricalData.Count - BufferSize);

                for (int i = start; i < _vm.HistoricalData.Count; i++)
                {
                    var historicalValues = _vm.HistoricalData[i];

                    _xBuffer[_writeIndex] = historicalValues.Time;
                    _processBufferFirstPT100[_writeIndex] = historicalValues.ProcessFirstPt100;
                    _processBufferSecondPT100[_writeIndex] = historicalValues.ProcessSecondPt100;
                    _setPointBuffer[_writeIndex] = historicalValues.SetPoint;

                    _writeIndex++;

                    if (_writeIndex >= BufferSize)
                    {
                        _writeIndex = 0;
                        _bufferFull = true;
                    }
                }
            }
            else
            {
                // Punto inicial
                double now = DateTime.Now.ToOADate();
                _xBuffer[0] = now;
                _processBufferFirstPT100[0] = 0;
                _processBufferSecondPT100[0] = 0;
                _setPointBuffer[0] = 0;
                _writeIndex = 1;
            }

            // Crear señales ScottPlot
            FirstPT100 = ScatterPlot.Plot.Add.SignalXY(_xBuffer, _processBufferFirstPT100);
            FirstPT100.LegendText = "PT100-1 (PV)";
            FirstPT100.LineWidth = 2;
            FirstPT100.LineColor = Colors.Blue;

            SecondPT100 = ScatterPlot.Plot.Add.SignalXY(_xBuffer, _processBufferSecondPT100);
            SecondPT100.LegendText = "PT100-2 (PV)";
            SecondPT100.LineWidth = 2;
            SecondPT100.LineColor = Colors.Red;

            _setPointSignal = ScatterPlot.Plot.Add.SignalXY(_xBuffer, _setPointBuffer);
            _setPointSignal.LegendText = "SetPoint (SP)";
            _setPointSignal.LineWidth = 2;
            _setPointSignal.LineColor = Colors.Green;

            UpdateRenderIndexes();

            // Configuración visual
            ScatterPlot.Plot.Axes.DateTimeTicksBottom();
            ScatterPlot.Plot.YLabel("Temperature (°C)");
            ScatterPlot.Plot.ShowLegend();
            ScatterPlot.Plot.Axes.AutoScale();

            // Suscribirse a nuevos datos
            _vm.NewSample += OnNewSample;

            // Timer de refresco (UI independiente del PLC)
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            _refreshTimer.Tick += RefreshTick;
            _refreshTimer.Start();
        }

        private void OnNewSample(double time, float processFirstPT100, float processSecondPT100, float setPoint)
        {
            Dispatcher.Invoke(() =>
            {
                if (_xBuffer == null)
                    return;

                // Ya no usar DateTime.Now aquí
                double now = time;

                if (_bufferFull || _writeIndex > 0)
                {
                    int lastIndex = (_writeIndex - 1 + BufferSize) % BufferSize;

                    if (now <= _xBuffer[lastIndex])
                        now = _xBuffer[lastIndex] + 0.0000001;
                }

                _xBuffer[_writeIndex] = now;
                _processBufferFirstPT100[_writeIndex] = processFirstPT100;
                _processBufferSecondPT100[_writeIndex] = processSecondPT100;
                _setPointBuffer[_writeIndex] = setPoint;

                _writeIndex++;

                if (_writeIndex >= BufferSize)
                {
                    _writeIndex = 0;
                    _bufferFull = true;
                }

                UpdateRenderIndexes();
            });

        }

        private void UpdateRenderIndexes()
        {
            if (_bufferFull)
            {
                FirstPT100.MinRenderIndex = _writeIndex;
                FirstPT100.MaxRenderIndex = _writeIndex + BufferSize - 1;

                SecondPT100.MinRenderIndex = _writeIndex;
                SecondPT100.MaxRenderIndex = _writeIndex + BufferSize - 1;

                _setPointSignal.MinRenderIndex = _writeIndex;
                _setPointSignal.MaxRenderIndex = _writeIndex + BufferSize - 1;
            }
            else
            {
                int maxIndex = Math.Max(0, _writeIndex - 1);

                FirstPT100.MinRenderIndex = 0;
                FirstPT100.MaxRenderIndex = maxIndex;

                SecondPT100.MinRenderIndex = 0;
                SecondPT100.MaxRenderIndex = maxIndex;

                _setPointSignal.MinRenderIndex = 0;
                _setPointSignal.MaxRenderIndex = maxIndex;
            }
        }

        private void RefreshTick(object? sender, EventArgs e)
        {
            if (_xBuffer == null)
                return;

            try
            {
                double now = DateTime.Now.ToOADate();
                double windowStart = now - TimeSpan.FromMinutes(10).TotalDays;

                // Ventana deslizante tipo SCADA
                ScatterPlot.Plot.Axes.SetLimitsX(windowStart, now);

                // Autoescala Y solo cuando hay suficientes datos
                if (_bufferFull)
                    ScatterPlot.Plot.Axes.AutoScaleY();

                ScatterPlot.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Plot refresh error: {ex.Message}");
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_vm != null)
                _vm.NewSample -= OnNewSample;

            _refreshTimer?.Stop();
            _refreshTimer = null;
        }
    }
}