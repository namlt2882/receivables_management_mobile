using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Common.DataAnnotations;

namespace RCM.Mobile.Converters
{

    class IntToDateConverter : IPropertyConverter
    {
        public object Convert(object value)
        {
            return value!=null?Utility.ConvertIntToDateStringForView((int)value):"";
        }
        public object ConvertBack(object value)
        {
            return int.Parse(Utility.ConvertDatetimeToString((DateTime)value));
        }
    }
    
}
