using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace RCM.Mobile.Converters
{
    public class ReceivableStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
                return null;
            switch ((int)value)
            {
                case Constant.COLLECTION_STATUS_COLLECTION_CODE: return Color.FromHex("#21ba45");
                case Constant.COLLECTION_STATUS_CANCEL_CODE: return Color.FromHex("#db2828");
                case Constant.COLLECTION_STATUS_DONE_CODE: return Color.FromHex("#2185d0");
                case Constant.COLLECTION_STATUS_CLOSED_CODE: return Color.FromHex("#767676");
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   
}


