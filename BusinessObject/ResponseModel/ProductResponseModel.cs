using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class ProductResponseModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public double Cost { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Origin { get; set; } = string.Empty;

        public double Productivity { get; set; }

        public string Code { get; set; } = string.Empty;

        public int InStock { get; set; }
    }
}
