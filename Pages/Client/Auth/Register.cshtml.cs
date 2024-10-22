using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Client.Auth;

public class RegisterModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly ZStoreDbContext _context;

  public RegisterModel(ILogger<IndexModel> logger, ZStoreDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  [BindProperty]
  [Required]
  public string? Firstname { get; set; }

  [BindProperty]
  [Required]
  public string? Lastname { get; set; }

  [BindProperty]
  [EmailAddress]
  [Required]
  public string? Email { get; set; }
  [BindProperty]
  [Required]
  public string? Phone { get; set; }

  [BindProperty]
  [Required]
  public string? Username { get; set; }

  [BindProperty]
  [Required]
  [DataType(DataType.Password)]
  public string? Password { get; set; }

  [BindProperty]
  [Required]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string? ConfirmPassword { get; set; }

  public async Task<IActionResult> OnPostAsync()
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    // Check if the username or email already exists
    if (await _context.Users.AnyAsync(a => a.Email == Email || a.Username == Username))
    {
      ModelState.AddModelError(string.Empty, "The username or email is already in use.");
      return Page();
    }

    var user = new User
    {
      Firstname = Firstname!,
      Lastname = Lastname!,
      Email = Email!,
      Phone = Phone!,
      Username = Username!,
      Password = Utils.Utils.GetHash(Password!),
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Redirect to login page or another page after successful registration
    return RedirectToPage("/Client/Auth/Login");
  }
}
