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

        public static IEnumerable<FundFrame> FilterFundFrame(DateTime? start, DateTime? end, IEnumerable<FundFrame> frm)
        {
            if (start != null && end != null)
            {
                return frm.Where(x => x.Date >= start && x.Date <= end).ToList();
            }
            else
            {
                if (start != null)
                {
                    return frm.Where(x => x.Date >= start).ToList();
                }
                else
                {
                    if (end != null)
                    {
                        return frm.Where(x => x.Date <= end).ToList();
                    }
                    else
                    {
                        return frm;
                    }
                }
            }

        }

        public static double? ProfitInvInterval(DateTime? start, DateTime? end, double? amount, double? annualRate)
        {
            TimeSpan diff = end.Value - start.Value;
            double daysDiff = diff.Days;
            double? fv = amount * Math.Pow(1 + annualRate.Value / 365.25, daysDiff);

            return fv;
        }

        public static double? ProfitFundInterval(double? valStart, double? valEnd, double? amount)
        {
            return amount + amount * Math.Log(valEnd.Value / valStart.Value);
        }
    }
}