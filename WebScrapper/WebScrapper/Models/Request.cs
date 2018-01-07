using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebScrapper.Models
{
    [Table("Request")]
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public string NDCNo { get; set; }
        public string MDunits { get; set; }
        public string price { get; set; }
        public string ServiceDate { get; set; }
        public bool NursingHome { get; set; }
        public bool Brand { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public virtual Response Response { get; set; }
    }
}