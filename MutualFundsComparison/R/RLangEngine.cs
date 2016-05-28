using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RDotNet;
using System.Collections;
using System.Text;
using System.IO;
using System.Drawing;


namespace MutualFundsComparison.RLanguageEngine
{
    public class RLangEngine
    {
        REngine engine;
        int plotWidth = 800;
        int plotHeight = 600;

        public RLangEngine(string rPath)
        {
            REngine.SetEnvironmentVariables(rPath);
            engine = REngine.GetInstance();
        }

        public RLangEngine(string rPath, int plotWidth, int plotHeight)
        {
            REngine.SetEnvironmentVariables(rPath);
            engine = REngine.GetInstance();
            this.plotWidth = plotWidth;
            this.plotHeight = plotHeight;
        }


        public byte[] RenderChartToByteArray(string[] characterVector, double[] numericVector)
        {
            LoadData(characterVector, numericVector);
            string buildedScript = BuildScriptWithCairoGraphics().ToString();
            var ret = engine.Evaluate(buildedScript).AsList();
            byte[] arr = ret[0].AsRaw().ToArray();

            return arr;
        }

        public Bitmap RenderBitmap(byte[] arr)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(arr))
            {
                bmp = new Bitmap(ms);
            }

            return bmp;
        }


        private void LoadData(IList<string> characterVector, IList<double> numericVector)
        {
            DataFrame frm = engine.CreateDataFrame(new IEnumerable[] { characterVector, numericVector }, new string[] { "date", "value" }, stringsAsFactors: false);
            engine.SetSymbol("input.data", frm);
        }

        private StringBuilder BuildScriptWithCairoGraphics()
        {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine(@"
            library(Cairo);
            library(png);
            library(ggplot2);
            message('cairo picture functions are enabled');
            captPicture<-function(dev, page){
                locPictures<-rStatServerCapturedPictures;
                pict <-png::writePNG(image = Cairo::Cairo.capture(dev), target = base::raw());
                locPictures[[length(locPictures) + 1]]<-pict;

                rStatServerCapturedPictures<<-locPictures;
            }");

            scriptBuilder.AppendLine(@"rStatServerCapturedPictures <- list();");
            scriptBuilder.AppendLine(string.Format(@"dev <- Cairo::Cairo(width = {0}, height = {1}, type = 'raster');", plotWidth, plotHeight));
            scriptBuilder.AppendLine(@"Cairo::Cairo.onSave(device = dev, onSave = captPicture);");

            scriptBuilder.AppendLine(@"input.data$date <- as.Date(strptime(input.data$date, '%Y-%m-%d'))");
            scriptBuilder.AppendLine(@"min.date <- as.character(min(input.data$date))");
            scriptBuilder.AppendLine(@"max.date <- as.character(max(input.data$date))");

            scriptBuilder.AppendLine(@"p <- ggplot(input.data, aes(date, value)) +");
            scriptBuilder.AppendLine(@"geom_line(aes(group = as.factor(1)), size = 1.0, colour = 'red', alpha = 0.5) +");
            scriptBuilder.AppendLine(@"xlab('Date') + ylab('Value') +");
            scriptBuilder.AppendLine(@"ggtitle(bquote(atop(bold('Time series for mutual fund'), atop(paste('Data from: ', .(min.date), ' to: ', .(max.date))))))");

            scriptBuilder.AppendLine("print(p)");
            scriptBuilder.AppendLine(@"dev.off();");
            scriptBuilder.AppendLine(@"output.data <- rStatServerCapturedPictures");

            return scriptBuilder;
        }
    }
}