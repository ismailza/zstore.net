using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zstore.net.Models;

public class Category
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Name { get; set; }

  public string? Description { get; set; }

  [Required]
  public required string Type { get; set; }

  public long? ParentCategoryId { get; set; }

  [ForeignKey("ParentCategoryId")]
  public Category? ParentCategory { get; set; }
}
