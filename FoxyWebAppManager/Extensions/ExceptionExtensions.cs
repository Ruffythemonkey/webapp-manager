using FoxyWebAppManager.ContentDialogs;

namespace FoxyWebAppManager.Extensions
{
    public static class ExceptionExtensions
    {
        extension(Exception exception)
        {
            public async Task ShowMessageUIAsync()
            {
                var MessageDialog = new DialogException() { Message = exception.Message};
                await MessageDialog.ContentDialog.ShowAsync();
            }
        }
    }
}
