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
            CompareModel compare = new CompareModel()
            {
                DateFrom = (DateTime?)Session["DateFrom"],
                DateTo = (DateTime?)Session["DateTo"]
            };

            return View(compare);
        }

        public ActionResult Investment(DateTime? dateFrom, DateTime? dateTo, double amount, double rate)
        {
            Session["DateFrom"] = dateFrom;
            Session["DateTo"] = dateTo;

            var frm = (IList<FundFrame>)DataHelpers.FilterFundFrame(dateFrom, dateTo, (IEnumerable<FundFrame>)Session["FundFrame"]);

            if (frm == null)
            {
                throw new Exception("Empty array");
            }

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
                    ProfitFund = DataHelpers.ProfitFundInterval(prevVal, nextVal, i == 1 ? compare.Amount : ret[i - 2].ProfitFund),
                    ProfitInv = DataHelpers.ProfitInvInterval(prevDate, nextDate, compare.Amount, compare.Rate)
                };

                ret.Add(ts);
            }

            return ret;
        }


    }
}