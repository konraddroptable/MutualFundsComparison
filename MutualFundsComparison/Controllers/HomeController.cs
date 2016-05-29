using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MutualFundsComparison.Helpers;
using MutualFundsComparison.Models;
using MutualFundsComparison.RLanguageEngine;
using System.Globalization;


namespace MutualFundsComparison.Controllers
{
    public class HomeController : Controller
    {
        //string rHomePath = @"C:\Program Files\R\R-3.3.0\bin\i386";

        public ActionResult Index()
        {

            HomeModel home = new HomeModel()
            {
                DateFrom = (DateTime?)Session["DateFrom"],
                DateTo = (DateTime?)Session["DateTo"]
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

            //home.ByteArray = RenderTimeSeriesChart(this.rHomePath, home.DataFrame);

            return PartialView("~/Views/Home/Frames.cshtml", home);
        }

        public ActionResult BackToIndex()
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
                        string line = string.Empty;
                        Stream stream = file.InputStream;
                        StreamReader sr = new StreamReader(stream);

                        line = sr.ReadLine();
                        var head = line.Split(',');

                        while((line = sr.ReadLine()) != null)
                        {
                            var sp = line.Split(',');
                            db.FundFrame.Local.Add(new FundFrame { Date = DataHelpers.ToDateTime(sp[0]), Value = DataHelpers.ToDouble(sp[1]) });                            
                        }

                        Session["FundFrame"] = db.FundFrame.Local;
                        home.DataFrame = DataHelpers.FilterFundFrame(dateFrom, dateTo, db.FundFrame.Local);
                        //home.ByteArray = RenderTimeSeriesChart(this.rHomePath, home.DataFrame);

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


        //Not in use
        //due to weak performance on server
        //hint: write console app as R Server and execute R script through the server
        //private byte[] RenderTimeSeriesChart(string rHomePath, IEnumerable<FundFrame> frm)
        //{
        //    RLangEngine engine = new RLangEngine(rHomePath);
        //    byte[] arr = engine.RenderChartToByteArray(
        //        frm.Select(x => Convert.ToString(x.Date)).ToArray(),
        //        frm.Select(x => Convert.ToDouble(x.Value)).ToArray());

        //    return arr;
        //}

    }
}