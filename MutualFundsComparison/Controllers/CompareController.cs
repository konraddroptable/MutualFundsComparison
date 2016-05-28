using MutualFundsComparison.Models;
using System;
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


        public ActionResult Compare(DateTime? dateFrom, DateTime? dateTo, double amount, double rate)
        {
            CompareModel compare = new CompareModel()
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                Amount = amount,
                Rate = rate,
                DataFrame = (IEnumerable<FundFrame>)Session["FundFrame"]
            };


            return View(compare);
        }
    }
}