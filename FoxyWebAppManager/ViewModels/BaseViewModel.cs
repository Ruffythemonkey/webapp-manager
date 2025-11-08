using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Contracts.ViewModels;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System.Security.AccessControl;
using System.Windows.Input;

namespace FoxyWebAppManager.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, INavigationAware
    {
        public DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        
        [ObservableProperty]
        public partial bool IsBussy { get; set; }

        public abstract void OnNavigatedFrom();
        public abstract void OnNavigatedTo(object parameter);

        /// <summary>
        /// Create OnProperty Changed for Extensions usefull
        /// </summary>
        /// <param name="propName"></param>
        public void PropChanged(string propName)
            => this.OnPropertyChanged(propName);

    }
}
