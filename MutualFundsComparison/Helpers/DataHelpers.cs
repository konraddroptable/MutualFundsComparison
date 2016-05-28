using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MutualFundsComparison.Helpers
{
    public static class DataHelpers
    {
        public static DateTime ToDateTime(string s)
        {
            string[] tab = Regex.Split(Regex.Replace(s, @"\/|\.|\:|\\", @"-"), "-");

            return new DateTime(Convert.ToInt32(tab[2]), Convert.ToInt32(tab[1]), Convert.ToInt32(tab[0]));
        }

        public static double ToDouble(string s)
        {
            return Convert.ToDouble(s, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}