using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zstore.net.Models
{
  public class OrderItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public required long OrderId { get; set; }

    [Required]
    public required long ProductId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int Quantity { get; set; }

    [Required]
    public required double Price { get; set; }

    [Required]
    [NotMapped]
    [Range(0, Double.MaxValue)]
    public required double Total { get; set; }

    [ForeignKey("OrderId")]
    public Order? Order { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    
  }
}
