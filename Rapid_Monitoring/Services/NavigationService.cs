using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using Lab_Stenter_Dryer.Model;
using Lab_Stenter_Dryer.Infrastructure.Base;

namespace Lab_Stenter_Dryer.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, Type> _viewModelToViewMap;
        private Dictionary<Type, Func<ViewModelBase>> _viewModelFactories;
        private readonly Stack<object> _navigationStack;
        private ContentControl _contentControl;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModelToViewMap = new Dictionary<Type, Type>();
            _navigationStack = new Stack<object>();
        }

        // Register ViewModel-View mappings
        public void RegisterMapping<TViewModel, TView>()
            where TViewModel : class
            where TView : FrameworkElement
        {
            _viewModelToViewMap[typeof(TViewModel)] = typeof(TView);
        }

        // Set the ContentControl container for navigation
        public void SetContentControl(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            NavigateTo<TViewModel>(null);
        }

        public void NavigateTo<TViewModel>(object parameter) where TViewModel : class
        {
            if (_contentControl == null)
                throw new InvalidOperationException("ContentControl not set. Call SetContentControl first.");

            var viewModelType = typeof(TViewModel);

            if (!_viewModelToViewMap.ContainsKey(viewModelType))
                throw new InvalidOperationException($"No view registered for {viewModelType.Name}");

            var viewType = _viewModelToViewMap[viewModelType];
            var view = Activator.CreateInstance(viewType) as FrameworkElement;
            // var viewModel =  Activator.CreateInstance(viewModelType);
            var viewModel = _serviceProvider.GetRequiredService(viewModelType); //change

            // If ViewModel has a parameter property, set it
            if (parameter != null)
            {
                var paramProp = viewModelType.GetProperty("Parameter");
                paramProp?.SetValue(viewModel, parameter);
            }

            view.DataContext = viewModel;

            // Save current view to navigation stack
            if (_contentControl.Content != null)
            {
                _navigationStack.Push(_contentControl.Content);
            }

            _contentControl.Content = view;
        }

        public void OpenWindow<TViewModel>() where TViewModel : class
        {
            OpenWindow<TViewModel>(null);
        }

        public void OpenWindow<TViewModel>(object parameter) where TViewModel : class
        {
            var window = CreateWindow<TViewModel>(parameter);
            window.Show();
        }

        public bool? ShowDialog<TViewModel>() where TViewModel : class
        {
            return ShowDialog<TViewModel>(null);
        }

        public bool? ShowDialog<TViewModel>(object parameter) where TViewModel : class
        {
            var window = CreateWindow<TViewModel>(parameter);
            return window.ShowDialog();
        }

        private Window CreateWindow<TViewModel>(object parameter) where TViewModel : class
        {
            var viewModelType = typeof(TViewModel);

            if (!_viewModelToViewMap.ContainsKey(viewModelType))
                throw new InvalidOperationException($"No view registered for {viewModelType.Name}");

            var viewType = _viewModelToViewMap[viewModelType];
            var view = Activator.CreateInstance(viewType);
            // var viewModel = Activator.CreateInstance(viewModelType);
            var viewModel = _serviceProvider.GetRequiredService(viewModelType); //change

            // If ViewModel has a parameter property, set it
            if (parameter != null)
            {
                var paramProp = viewModelType.GetProperty("Parameter");
                paramProp?.SetValue(viewModel, parameter);
            }

            Window window;

            if (view is Window)
            {
                window = view as Window;
                window.DataContext = viewModel;
            }
            else if (view is UserControl userControl)
            {
                // Wrap UserControl in a Window
                window = new Window
                {
                    Content = userControl,
                    DataContext = viewModel,
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                userControl.DataContext = viewModel;
            }
            else
            {
                throw new InvalidOperationException("View must be a Window or UserControl");
            }

            return window;
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                _contentControl.Content = _navigationStack.Pop();
            }
        }

        public bool CanGoBack => _navigationStack.Count > 0;

        public void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.IsActive);
            window?.Close();
        }
    }
}
