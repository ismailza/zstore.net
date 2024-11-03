using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zstore.net.Models;

public class Address
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public long Id { get; set; }

  [Required]
  public required string Street { get; set; }

  [Required]
  public required string City { get; set; }

  [Required]
  public required string State { get; set; }

  [Required]
  public required string Zip { get; set; }

  [Required]
  public required string Country { get; set; }

  public Address()
  {
  }

  public Address(string street, string city, string state, string zip, string country)
  {
    Street = street;
    City = city;
    State = state;
    Zip = zip;
    Country = country;
  }
}
