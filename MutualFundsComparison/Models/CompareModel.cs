using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MutualFundsComparison.Models
{
    public class CompareModel
    {
        [Key]
        public int compareId { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double? Amount { get; set; }
        public double? Rate { get; set; }
        public IList<FundFrame> DataFrame { get; set; }

        public IList<TimeSeries> TimeSeries { get; set; }
    }

    public class TimeSeries
    {
        public DateTime? Date { get; set; }
        public double? ProfitFund { get; set; }
        public double? ProfitInv { get; set; }
    }
}