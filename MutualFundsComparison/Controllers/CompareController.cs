using MutualFundsComparison.Helpers;
using MutualFundsComparison.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MutualFundsComparison.Controllers
{
    public class CompareController : Controller
    {
        public ActionResult Index()
        {
            CompareModel compare = new CompareModel();

            return View(compare);
        }

        public ActionResult Investment(DateTime? dateFrom, DateTime? dateTo, double amount, double rate)
        {
            var frm = (IList<FundFrame>)DataHelpers.FilterFundFrame(dateFrom, dateTo, (IEnumerable<FundFrame>)Session["FundFrame"]);
            DateTime? minDate = dateFrom.HasValue ? dateFrom : frm.Min(x => x.Date);
            DateTime? maxDate = dateTo.HasValue ? dateTo : frm.Max(x => x.Date);

            CompareModel compare = new CompareModel()
            {
                DateFrom = minDate,
                DateTo = maxDate,
                Amount = amount,
                Rate = rate,
                DataFrame = frm
            };

            compare.TimeSeries = GenerateInvestmentComparison(compare);

            return PartialView("~/Views/Compare/Chart.cshtml", compare);
        }

        private List<TimeSeries> GenerateInvestmentComparison(CompareModel compare)
        {
            var ret = new List<TimeSeries>();
            DateTime? prevDate = compare.DataFrame[0].Date;

            for (int i = 1; i < compare.DataFrame.Count(); i++)
            {
                DateTime? nextDate = compare.DataFrame[i].Date;
                double? prevVal = compare.DataFrame[i - 1].Value;
                double? nextVal = compare.DataFrame[i].Value;

                TimeSeries ts = new TimeSeries()
                {
                    Date = nextDate,
                    ProfitFund = ProfitFundInterval(prevVal, nextVal, i == 1 ? compare.Amount : ret[i - 2].ProfitFund),
                    ProfitInv = ProfitInvInterval(prevDate, nextDate, compare.Amount, compare.Rate)
                };

                ret.Add(ts);
            }

            return ret;
        }

        private double? ProfitInvInterval(DateTime? start, DateTime? end, double? amount, double? annualRate)
        {
            TimeSpan diff = end.Value - start.Value;
            double daysDiff = diff.Days;
            double? fv = amount * Math.Pow(1 + annualRate.Value / 365.25, daysDiff);

            return fv;
        }

        private double? ProfitFundInterval(double? valStart, double? valEnd, double? amount)
        {
            return amount + amount * Math.Log(valEnd.Value / valStart.Value);
        }
    }
}