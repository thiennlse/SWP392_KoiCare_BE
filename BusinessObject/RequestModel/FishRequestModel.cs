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
    public  class FishRequestModel
    {
        public int PoolId { get; set; }

       
        public int FoodId { get; set; }

        
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        
        [StringLength(255)]
        public string Image { get; set; } = string.Empty;

        
        [Range(0, double.MaxValue)]
        public double Size { get; set; }

        
        [Range(0, double.MaxValue)]
        public double Weight { get; set; }

        
        [Range(0, int.MaxValue)]
        public DateTime Dob { get; set; }

       
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;


        [StringLength(100)]
        public string Origin { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual Food? Food { get; set; }
        [JsonIgnore]
        public virtual Pool? Pool { get; set; } = null!;
    }
}
