using System;
using System.ComponentModel.DataAnnotations;

namespace MvcDemoSite.Models
{
    public class CurrencyRate
    {   
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = "{0:##.0000}")]
        public decimal Rate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}