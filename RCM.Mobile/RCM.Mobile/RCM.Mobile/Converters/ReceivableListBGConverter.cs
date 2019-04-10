using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{

    public class ReceivableListBGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            switch ((int)value)
            {
                case Constant.COLLECTION_STATUS_COLLECTION_CODE: return Color.FromHex("#d7fccf");
                case Constant.COLLECTION_STATUS_CANCEL_CODE: return Color.FromHex("#f4caca");
                case Constant.COLLECTION_STATUS_DONE_CODE: return Color.FromHex("#edf6fc");
                case Constant.COLLECTION_STATUS_CLOSED_CODE: return Color.FromHex("#eae6e6");
            }
            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
