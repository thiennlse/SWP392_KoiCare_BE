﻿using BusinessObject.Models;
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
    public class OrderRequestModel
    {
        public List<int> ProductId { get; set; }
        public List<int> Quantity { get; set; }
        public double TotalCost { get; set; }
        public DateTime CloseDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }

}
