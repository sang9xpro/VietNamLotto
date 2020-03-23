using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class DateUtils
    {
        public static string FormatDateTime(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy HH:mm:ss");
        }
        public static string FormatDate(this DateTime date)
        {
            return date.ToString("dd-MM-yyyy");
        }
        public static string FormatYYYYMMDD(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
       public static string FormatYYYYMMDD(this string date)
        {
            return DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
