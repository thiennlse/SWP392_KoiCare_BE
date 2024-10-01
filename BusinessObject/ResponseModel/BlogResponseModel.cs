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
    public  class BlogResponseModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfPublish { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        
        public virtual Member? Member { get; set; }
    }
}
