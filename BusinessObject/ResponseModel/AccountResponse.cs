using BusinessObject.Models;
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
        public ICollection<UserSubcriptions> Subcriptions { get; set; } = new List<UserSubcriptions>();
        public string? Role { get; set; }
    }
}
