using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MutualFundsComparison.Controllers
{
    public class HomeController : Controller
    {
        MutualFundsEntities db = new MutualFundsEntities();

        public ActionResult Index()
        {
            return View(db.FundFrame.ToList());
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (ModelState.IsValid)
            {
                if(Request.Files.Count > 0)
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
                            db.FundFrame.Add(new FundFrame { Date = sp[0], Value = Convert.ToDouble(sp[1], System.Globalization.NumberFormatInfo.InvariantInfo) });
                        }

                        return View("~/Views/Home/Index.cshtml", db.FundFrame.Local.ToList());
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

            return View("~/Views/Home/Index.cshtml", db.FundFrame.Local.ToList());
        }
    }
}