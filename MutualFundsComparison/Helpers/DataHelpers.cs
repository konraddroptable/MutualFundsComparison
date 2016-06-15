using System;
using System.Collections.Generic;
using System.IO;
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

        public static IEnumerable<FundFrame> UploadFile(HttpPostedFileBase file)
        {
            List<FundFrame> frm = new List<FundFrame>();
            string line = string.Empty;
            Stream stream = file.InputStream;
            StreamReader sr = new StreamReader(stream);

            line = sr.ReadLine();
            var head = line.Split(',');

            while ((line = sr.ReadLine()) != null)
            {
                var sp = line.Split(',');
                frm.Add(new FundFrame { Date = ToDateTime(sp[0]), Value = ToDouble(sp[1]) });
            }

            return frm;
        }

        public static IEnumerable<FundFrame> FilterFundFrame(DateTime? start, DateTime? end, IEnumerable<FundFrame> frm)
        {
            if (frm != null)
            {
                return frm.Where(x => 
                                    (start.HasValue ? x.Date >= start.Value : x.Date >= frm.Min(y => y.Date.Value)) &&
                                    (end.HasValue ? x.Date <= end.Value : x.Date <= frm.Max(y => y.Date.Value)));
            }

            return frm;
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