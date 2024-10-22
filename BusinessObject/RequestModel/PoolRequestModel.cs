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
    public class PoolRequestModel
    {
        public string Name { get; set; } = string.Empty;
        public double Size { get; set; }
        public double Depth { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
