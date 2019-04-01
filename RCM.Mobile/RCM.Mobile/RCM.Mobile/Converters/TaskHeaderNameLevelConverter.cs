using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView;
using Xamarin.Forms;

namespace RCM.Mobile.Converters
{

    public class TaskHeaderNameLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            var header = (GroupHeaderContext)value;
            if (header.Level == 1)
            {
                switch ((int)header.Key)
                {
                    case Constant.ACTION_REPORT_CODE: return Constant.ACTION_REPORT;
                    case Constant.ACTION_VISIT_CODE: return Constant.ACTION_VISIT;
                }
            }
            else if (header.Level == 2)
            {
                var time = (TimeSpan)header.Key;
                return $"{time.Hours}:{time.Minutes}";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
