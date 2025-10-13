using CommunityToolkit.Mvvm.ComponentModel;
using FoxyWebAppManager.Contracts.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.Storage.Pickers;

namespace FoxyWebAppManager.ViewModels
{
    public abstract partial class BaseViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        public partial bool IsBussy { get; set; }

        public abstract void OnNavigatedFrom();
        public abstract void OnNavigatedTo(object parameter);

    }
}
