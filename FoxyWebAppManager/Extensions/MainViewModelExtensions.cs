using FoxyWebAppManager.Models;
using FoxyWebAppManager.ViewModels;
using Microsoft.Windows.Storage.Pickers;

namespace FoxyWebAppManager.Extensions
{
    public static class MainViewModelExtensions
    {
        extension(MainViewModel viewModel) 
        {
            /// <summary>
            /// Open New FF Path and save it in a json in LocalAppFolder
            /// </summary>
            /// <returns></returns>
            public async Task OpenFireFoxPathEx()
            {
                var openPicker = new FileOpenPicker(App.MainWindow.AppWindow.Id);
                openPicker.FileTypeFilter.Add(".exe");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var ffd = new FireFoxData() { Path = file.Path };
                    FireFoxPathExtensions.Save(ffd);
                    viewModel.FireFoxData = ffd;
                }
            }

            /// <summary>
            /// OpenDialog For Lnk Icons
            /// </summary>
            /// <returns></returns>
            public async Task OpenIconPathEx()
            {
                var openPicker = new FileOpenPicker(App.MainWindow.AppWindow.Id);
                openPicker.FileTypeFilter.Add(".ico");
                openPicker.FileTypeFilter.Add(".png");
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".jpeg");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    viewModel.FavIcon = file.Path;
                }
            }


        }
    }
}
