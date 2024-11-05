using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using zstore.net.Enums;

namespace zstore.net.Models;

public class Order
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required long UserId { get; set; }

  [Required]
  public required long AddressId { get; set; }

  [NotMapped]
  public double Total => OrderItems.Sum(item => item.Price * item.Quantity);

  [Required]
  public OrderStatus Status { get; set; } = OrderStatus.PENDING;

  [Required]
  public DateTime CreatedAt { get; set; } = DateTime.Now;

  [Required]
  public DateTime UpdatedAt { get; set; }

  [ForeignKey("UserId")]
  public User? User { get; set; }

  [ForeignKey("AddressId")]
  public Address? Address { get; set; }

  [Required]
  public List<OrderItem> OrderItems { get; set; } = [];
}
