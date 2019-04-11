using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{
    public class ActionTypeConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            var status = (int)value;
            switch (status)
            {
                case Constant.ACTION_VISIT_CODE: return "house_visiting";
                case Constant.ACTION_REPORT_CODE: return "iconfinder_27_Report_24";
                case Constant.ACTION_NOTIFICATION_CODE: return "house_visiting";
            }
            return "Transparency500";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
