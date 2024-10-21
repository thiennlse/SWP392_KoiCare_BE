using BusinessObject.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Product : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double Cost { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public double Productivity { get; set; }
    public string Code { get; set; } = string.Empty;

    public int InStock { get; set; }

    [JsonIgnore]
    public virtual Member? User { get; set; }
    [JsonIgnore]
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
}
