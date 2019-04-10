using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;

namespace RCM.Mobile.Converters
{
    public class TypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (int)value;
            switch (type)
            {
                case Constant.ACTION_VISIT_CODE: return "";
                case Constant.ACTION_REPORT_CODE: return "";
                case Constant.ACTION_PHONECALL_CODE: return "";
                case Constant.ACTION_SMS_CODE: return "";

            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
