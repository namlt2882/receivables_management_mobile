
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{
    class ReceivableStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var status = (int)value;
            if (status == Constant.COLLECTION_STATUS_COLLECTION_CODE)
                return "Collecting";
            if (status == Constant.COLLECTION_STATUS_CLOSED_CODE)
                return "Closed";
            if (status == Constant.COLLECTION_STATUS_CANCEL_CODE)
                return "Cancel";
            return "Other";
            //var receivable = (Receivable)value;
            //if (receivable.IsConfirmed)
            //    return "Closed";
            //if (receivable.CollectionProgressStatus == Constant.COLLECTION_STATUS_COLLECTION_CODE)
            //    return "Collecting";
            //return "Not Confirmed";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
