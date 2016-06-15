using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MutualFundsComparison.Helpers;
using MutualFundsComparison.Models;


namespace MutualFundsComparison.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DateTime? dateFrom = (DateTime?)Session["DateFrom"];
            DateTime? dateTo = (DateTime?)Session["DateTo"];

            HomeModel home = new HomeModel()
            {
                DataFrame = DataHelpers.FilterFundFrame(dateFrom, dateTo, (IEnumerable<FundFrame>)Session["FundFrame"]),
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            return View("~/Views/Home/Index.cshtml", home);
        }

        public ActionResult Filter(DateTime? dateFrom, DateTime? dateTo)
        {
            Session["DateFrom"] = dateFrom;
            Session["DateTo"] = dateTo;

            HomeModel home = new HomeModel()
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                DataFrame = DataHelpers.FilterFundFrame(dateFrom, dateTo, (IEnumerable<FundFrame>)Session["FundFrame"])
            };

            return PartialView("~/Views/Home/Frames.cshtml", home);
        }

        [HttpPost]
        public ActionResult Upload(DateTime? dateFrom, DateTime? dateTo)
        {
            Session["DateFrom"] = dateFrom;
            Session["DateTo"] = dateTo;

            HomeModel home = new HomeModel()
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            };

            if (ModelState.IsValid)
            {
                MutualFundsEntities db = new MutualFundsEntities();

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.FileName.EndsWith(".csv"))
                    {
                        db.FundFrame.AddRange(DataHelpers.UploadFile(file));
                        Session["FundFrame"] = db.FundFrame.Local;
                        home.DataFrame = DataHelpers.FilterFundFrame(dateFrom, dateTo, db.FundFrame.Local);

                        return View("~/Views/Home/Index.cshtml", home);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please upload your file");
                }
            }

            return View("~/Views/Home/Index.cshtml", home);
        }
    }
}