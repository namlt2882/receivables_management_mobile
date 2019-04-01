using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{

    public class TaskTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var status = (int)value;
            switch (status)
            {
                case Constant.ACTION_NOTIFICATION_CODE: return Constant.ACTION_NOTIFICATION;
                case Constant.ACTION_REPORT_CODE: return Constant.ACTION_REPORT;
                case Constant.ACTION_VISIT_CODE: return Constant.ACTION_VISIT;
            }
            return "Other";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
