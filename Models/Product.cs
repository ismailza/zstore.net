using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zstore.net.Models;

public class Product
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Name { get; set; }

  public string? Description { get; set; }

  [Required]
  [Range(0, Double.MaxValue)]
  public required double Price { get; set; }

  [Range(0, int.MaxValue)]
  [Required]
  public required int Quantity { get; set; }

  [Required]
  public required long CategoryId { get; set; }

  [ForeignKey("CategoryId")]
  public Category? Category { get; set; }

  public string? ImageUrl { get; set; }
}
