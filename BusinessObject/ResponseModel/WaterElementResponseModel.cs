using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class WaterElementResponseModel
    {
        public double StandardTemperature { get; set; }
        public double StandardSalt { get; set; }
        public double StandardNo2 { get; set; }
        public double StandardNo3 { get; set; }
        public double StandardPo4 { get; set; }
        public double StandardPH { get; set; }
        public double StandardO2 { get; set; }

        public List<int> listProductId  { get; set; } = new List<int>();
    }
}
