using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace FoxyWebAppManager.Converters
{
    public class VisibiltyNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible ? Visibility.Collapsed: Visibility.Visible;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
