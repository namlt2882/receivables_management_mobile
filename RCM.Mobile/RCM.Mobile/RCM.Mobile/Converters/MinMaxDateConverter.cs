using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{
    public class MinMaxDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? Utility.ConvertIntToDateStringForViewReverse(Utility.ConvertDatimeToInt((DateTime)value)) : "";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
