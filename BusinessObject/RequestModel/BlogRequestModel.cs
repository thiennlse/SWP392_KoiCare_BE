using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class BlogRequestModel
    {
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublish { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }

    }
}
