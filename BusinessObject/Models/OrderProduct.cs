using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class OrderProduct : BaseEntity
    {

        public int? OrderId { get; set; }
        [JsonIgnore]
        public virtual Order? Order { get; set; }
        public int? SubcriptionId { get; set; }
        public virtual Subcriptions? Subcriptions { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
