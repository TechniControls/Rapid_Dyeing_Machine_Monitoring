using System;
using System.Collections.Generic;
using System.Text;

namespace Rapid_Monitoring.Model
{
    public interface INavigationService
    {
        // Navigate to a UserControl view within the main window
        void NavigateTo<TViewModel>() where TViewModel : class;
        void NavigateTo<TViewModel>(object parameter) where TViewModel : class;

        // Open a new Window
        void OpenWindow<TViewModel>() where TViewModel : class;
        void OpenWindow<TViewModel>(object parameter) where TViewModel : class;

        // Show dialog window
        bool? ShowDialog<TViewModel>() where TViewModel : class;
        bool? ShowDialog<TViewModel>(object parameter) where TViewModel : class;

        // Navigate back
        void GoBack();
        bool CanGoBack { get; }

        // Close current window
        void CloseWindow();
    }
}
