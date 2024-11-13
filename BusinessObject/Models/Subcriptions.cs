
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public class Subcriptions : BaseEntity
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        [JsonIgnore]
        public ICollection<UserSubcriptions> UserSubcriptions { get; set; } = new List<UserSubcriptions>();
    }
}
