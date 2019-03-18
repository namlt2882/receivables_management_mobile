using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{
    public class MoneyOnTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value == null)
            //    return null;

            //var receivable = value as Receivable;
            //return receivable.PrepaidAmount + "/" + receivable.DebtAmount;
            throw new NotImplementedException();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
