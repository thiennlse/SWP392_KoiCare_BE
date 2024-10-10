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
   public  class FoodRequestModel
    {
        
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        
        [Range(0, double.MaxValue)]
        public double Weight { get; set; }
        
    }
}
