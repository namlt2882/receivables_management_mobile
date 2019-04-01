using System;
using System.Globalization;
using Xamarin.Forms;


namespace RCM.Mobile.Converters
{
    public class LevelToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            int levelparse = 0 - (int)value;
            var level = (uint)levelparse;
            return new Thickness((int)value * 10, 0, 0, 0);
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
