using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebScrapper.Models
{
    [Table("Response")]
    public class Response
    {
        [Key, ForeignKey("Request")]
        public int RequestId { get; set; }
        public string LabelName { get; set; }
        public DateTime PriceDate { get; set; }
        public int MDunit { get; set; }
        public double UnitPrice { get; set; }
        public double Product { get; set; }
        public double TotalOfIngrediant { get; set; }
        public double MediCalDespensingFeeOf { get; set; }
        public double MediCalDespensingFee { get; set; }
        public double EqualsSubtotal { get; set; }
        public double CustomaryPrice { get; set; }
        public double reduction { get; set; }
        public double ProductTotal { get; set; }

        public virtual Request Request { get; set; }
    }
}