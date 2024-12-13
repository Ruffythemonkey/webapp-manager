using Microsoft.UI.Xaml.Data;

namespace webapp_manager.Converters
{
    public class InfobarOpenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return string.IsNullOrEmpty((string)value) ? false : true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
