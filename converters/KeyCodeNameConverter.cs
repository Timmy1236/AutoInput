using System.Globalization;
using System.Windows.Data;

namespace AutoInput.converters
{
    public class KeyCodeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            string name = value.ToString();

            if (name.StartsWith("Vc", StringComparison.OrdinalIgnoreCase) && name.Length > 2)
            {
                return name.Substring(2);
            }

            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}