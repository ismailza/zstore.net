using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;

namespace zstore.net.Models;

public partial class Product
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Name { get; set; }

  public string? Slug { get; set; }

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

  [NotMapped]
  public IFormFile? Image { get; set; }

  public string? ImageUri { get; set; }

  public DateTime CreatedAt { get; set; } = DateTime.Now;

  public DateTime UpdatedAt { get; set; } = DateTime.Now;

  /**
    * Generate a unique slug for the product based on its name.
    * 
    * @param existingProducts A list of existing products to check for uniqueness.
    */
  public void GenerateSlug(IEnumerable<Product> existingProducts)
  {
    string baseSlug = MyRegex().Replace(Name.ToLower(), "")
                           .Trim()
                           .Replace(" ", "-");

    if (string.IsNullOrEmpty(baseSlug))
    {
      baseSlug = "product";
    }

    string uniqueSlug = baseSlug;
    int counter = 1;

    while (existingProducts.Any(c => c.Slug == uniqueSlug && c.Id != Id))
    {
      uniqueSlug = $"{baseSlug}-{counter}";
      counter++;
    }

    Slug = uniqueSlug;
  }

  [GeneratedRegex(@"[^a-z0-9\s-]")]
  private static partial Regex MyRegex();
}
