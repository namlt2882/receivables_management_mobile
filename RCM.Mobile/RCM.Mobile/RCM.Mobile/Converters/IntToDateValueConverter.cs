using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace RCM.Mobile.Converters
{
    public class IntToDateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Utility.ConvertIntToDateStringForView((int)value) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty((string)value)) return "";
            string result = (string)value;
            return DateTime.ParseExact(result, "dd/MM/yyyy", CultureInfo.CurrentCulture); ;
        }
    }
}
