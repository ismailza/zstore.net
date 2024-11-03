using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;

namespace zstore.net.Pages.Client.Auth;

public class LoginModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly ZStoreDbContext _context;

  public LoginModel(ILogger<IndexModel> logger, ZStoreDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  [BindProperty]
  [Required(ErrorMessage = "Username is required")]
  public String Username { get; set; } = "";
  [BindProperty]
  [Required(ErrorMessage = "Password is required")]
  [DataType(DataType.Password)]
  public String Password { get; set; } = "";

  [BindProperty(SupportsGet = true)]
  public string? ReturnUrl { get; set; }

  public async Task<IActionResult> OnPostAsync()
  {
    var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == Username || a.Username == Username);
    if (user == null || !Utils.Utils.VerifyHash(Password, user.Password))
    {
      ModelState.AddModelError("Username", "Invalid username or password");
      return Page();
    }

    var claims = new List<Claim>
    {
      new(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
      new(ClaimTypes.Email, user.Email),
      new(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new(ClaimTypes.Role, "Client")
    };

    var claimsIdentity = new ClaimsIdentity(claims, "ClientAuth");
    var authProperties = new AuthenticationProperties
    {
      IsPersistent = true,
      ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
    };

    await HttpContext.SignInAsync("ClientAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
    {
      return LocalRedirect(ReturnUrl);
    }

    return RedirectToPage("/Client/Index");
  }

}