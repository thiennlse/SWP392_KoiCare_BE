using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public List<int> ProductId { get; set; }
        public double TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
