using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class FishResponseModel
    {
        public int Id { get; set; }

        public int PoolId { get; set; }
        public int FoodId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
        public double Size { get; set; }
        public double Weight { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
       
        public virtual Food? Food { get; set; }
        
        public virtual Pool? Pool { get; set; } = null!;
    }
}
