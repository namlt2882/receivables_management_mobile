using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Common.DataAnnotations;
using System.Text.RegularExpressions;


namespace RCM.Mobile.Converters
{
    class MoneyConverter : IPropertyConverter
    {
        public object Convert(object value)
        {
            return String.Format("{0:n0}", (decimal)value);

        }
        public object ConvertBack(object value)
        {
            return decimal.Parse(Regex.Replace((string)value, ",", ""));
        }
    }
   
}
