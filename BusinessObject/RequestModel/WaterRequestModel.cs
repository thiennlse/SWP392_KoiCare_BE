using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class WaterRequestModel
    {

        [Required]
        [Range(-273.15, double.MaxValue)]
        public double Temperature { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Salt { get; set; }

        [Required]
        [Range(0, 14)]
        public double Ph { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double O2 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double No2 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double No3 { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Po4 { get; set; }

       
    }
}
