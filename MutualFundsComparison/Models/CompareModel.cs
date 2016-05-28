using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MutualFundsComparison.Models
{
    public class CompareModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public IEnumerable<FundFrame> DataFrame { get; set; }
    }
}