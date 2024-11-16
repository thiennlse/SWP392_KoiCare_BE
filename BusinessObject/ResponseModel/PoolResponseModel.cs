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
    public class PoolResponseModel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public double Size { get; set; }

        public double Depth { get; set; }

        public double Volume { get; set; }

        public string Description { get; set; } = string.Empty;

        public int WaterId { get; set; }

        public virtual Member Member { get; set; } = null!;

        public virtual Waters Water { get; set; } = null!;

    }
}
