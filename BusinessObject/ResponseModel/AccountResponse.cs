using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel
{
    public class AccountResponse
    {
        public bool Success { get; set; }   
        public int? UserId {  get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }
}
