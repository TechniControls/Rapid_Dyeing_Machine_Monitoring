using Microsoft.Extensions.DependencyInjection;
using Rapid_Monitoring.Model;
using Rapid_Monitoring.ViewModel;
using Rapid_Monitoring.View;
using Rapid_Monitoring.Services;
using Rapid_Monitoring.Store;
using System.Configuration;
using System.Data;
using System.Windows;
using Rapid_Monitoring.Components;

namespace Rapid_Monitoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            //// 🔹 Bind PLC → TemperatureStore (UNA SOLA VEZ)
            //PlcService.BindTemperatureStore(
            //    _serviceProvider.GetRequiredService<TemperatureStore>()
            //);

            var nav = (NavigationService)_serviceProvider.GetRequiredService<INavigationService>();

            // 🔥 AQUÍ es donde deben registrarse
            nav.RegisterMapping<ProcessControlViewModel, ProcessControlView>();
            nav.RegisterMapping<TemperatureTrendViewModel, TemperatureTrendView>();
            nav.RegisterMapping<HomeViewModel, HomeView>();
            nav.RegisterMapping<ConnectionViewModel, ConnectionWindow>();

            nav.SetContentControl(mainWindow.NavigationContent);

            // Set the initial view after registering mappings
            nav.NavigateTo<HomeViewModel>();

            mainWindow.Show();

            base.OnStartup(e);

        }



        private void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<INavigationService, NavigationService>();

            // STORES (persistencia)
            services.AddSingleton<ConnectionStore>();

            // VIEWMODELS
            services.AddSingleton<NavigationBarViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<TemperatureTrendViewModel>();
            services.AddTransient<ProcessControlViewModel>();
            services.AddTransient<ConnectionViewModel>();
            services.AddTransient<HomeViewModel>();

            // SERVICES
            //services.AddSingleton<IConnectionService, ConnectionService>();

            // VIEWS
            services.AddSingleton<MainWindow>();
            services.AddSingleton<NavigationBar>(s => new NavigationBar()
            {
                DataContext = s.GetRequiredService<NavigationBarViewModel>()
            });

        }

    }
}