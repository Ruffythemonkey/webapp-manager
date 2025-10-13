using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Contracts.ViewModels;
using Microsoft.UI.Dispatching;

namespace FoxyWebAppManager.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, INavigationAware
    {
        public DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        
        [ObservableProperty]
        public partial bool IsBussy { get; set; }

        public abstract void OnNavigatedFrom();
        public abstract void OnNavigatedTo(object parameter);

    }
}
