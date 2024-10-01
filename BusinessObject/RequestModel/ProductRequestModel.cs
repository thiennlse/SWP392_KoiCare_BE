using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class ProductRequestModel
    {

        
        public int UserId { get; set; }

        
        
        public string Name { get; set; } = string.Empty;

        
        
        public double Cost { get; set; }

        
        
        public string Description { get; set; } = string.Empty;

        
        
        public string Origin { get; set; } = string.Empty;

        
        
        public double Productivity { get; set; }

        
        
        public string Code { get; set; } = string.Empty;

        
        
        public int InStock { get; set; }

      
    }
}
