using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MutualFundsComparison.Models
{
    public class HomeModel
    {
        [Key]
        public int homeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public byte[] ByteArray { get; set; }
        public IEnumerable<MutualFundsComparison.FundFrame> DataFrame { get; set; }
    }
}