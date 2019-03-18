using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RCM.Mobile.Helpers
{
    public static class Utility
    {
        //Ex Input: 700 to 07:00 or 1700 to 17:00
        public static string ConvertIntToTimeStringForView(int time)
        {
            string tmp = time.ToString();
            if (tmp.Length != 4 && tmp.Length != 3)
            {
                return null;
            }

            if (tmp.Length == 3)
            {
                tmp = "0" + tmp;
            }
            string result = tmp.Substring(0, 2) + ":" + tmp.Substring(2);

            return result;
        }

        //Ex Input: 19970214 to 14/02/1997.
        public static string ConvertIntToDateStringForView(int time)
        {
            string tmp = time.ToString();
            if (tmp.Length != 8)
            {
                return null;
            }

            string result = tmp.Substring(6, 2) + "/" + tmp.Substring(4, 2) + "/" + tmp.Substring(0, 4);
            return result;
        }

        //Ex Input: 700 to 07:00 or 1700 to 17:00
        public static TimeSpan ConvertIntToTimeSpan(int time)
        {
            string tmp = time.ToString();


            if (tmp.Length == 3)
            {
                tmp = "0" + tmp;
            }
            return new TimeSpan(int.Parse(tmp.Substring(0, 2)), int.Parse(tmp.Substring(2)), 0);
        }


        //Convert 19970214 to Datetime
        public static DateTime ConvertIntToDatetime(int time)
        {
            string tmp = time.ToString();
            return DateTime.ParseExact(tmp, Constant.DATE_FORMAT, CultureInfo.CurrentCulture);
        }

        public static string ConvertDatetimeToString(DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public static double GetGeoLocation()
        {
            return 0.0;
        }

       

    }
}
