using System.Globalization;
using System.Windows.Data;
using SharpHook.Data;

namespace AutoInput.converters
{
    public class MouseButtonNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MouseButton button)
            {
                return button switch
                {
                    MouseButton.NoButton => "Ninguno",
                    MouseButton.Button1 => "Clic Izquierdo",
                    MouseButton.Button2 => "Clic Derecho",
                    MouseButton.Button3 => "Clic Central",
                    MouseButton.Button4 => "Botón Lateral 1",
                    MouseButton.Button5 => "Botón Lateral 2",
                    _ => button.ToString()
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
