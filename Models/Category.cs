using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Text.RegularExpressions;

namespace zstore.net.Models;

public partial class Category
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Name { get; set; }

  public string? Slug { get; set; }

  [NotMapped]
  public IFormFile? Image { get; set; }

  public string? ImageUri { get; set; }

  public string? Description { get; set; }

  [Required]
  public required string Type { get; set; }

  public long? ParentCategoryId { get; set; }

  [ForeignKey("ParentCategoryId")]
  public Category? ParentCategory { get; set; }

  public ICollection<Category> SubCategories { get; set; } = [];

  public ICollection<Product> Products { get; set; } = [];

  /**
    * Generate a unique slug for the category based on its name.
    * 
    * @param existingCategories A list of existing categories to check for uniqueness.
    */
  public void GenerateSlug(IEnumerable<Category> existingCategories)
  {
    string baseSlug = MyRegex().Replace(Name.ToLower(), "")
                           .Trim()
                           .Replace(" ", "-");

    if (string.IsNullOrEmpty(baseSlug))
    {
      baseSlug = "category";
    }

    string uniqueSlug = baseSlug;
    int counter = 1;

    while (existingCategories.Any(c => c.Slug == uniqueSlug && c.Id != Id))
    {
      uniqueSlug = $"{baseSlug}-{counter}";
      counter++;
    }

    Slug = uniqueSlug;
  }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex MyRegex();
}
