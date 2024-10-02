using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zstore.net.Models;

public class User
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Firstname { get; set; }

  [Required]
  public required string Lastname { get; set; }

  [Required]
  [EmailAddress]
  public required string Email { get; set; }

  [Required]
  public required string Phone { get; set; }

  [Required]
  public required string Username { get; set; }

  [Required]
  public required string Password { get; set; }

  public long? AddressId { get; set; }

  [Required]
  public DateTime CreatedAt { get; set; } = DateTime.Now;

  [Required]
  public DateTime UpdatedAt { get; set; }

  [ForeignKey("AddressId")]
  public Address? Address { get; set; }
}
