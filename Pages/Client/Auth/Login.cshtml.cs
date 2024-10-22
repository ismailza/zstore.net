using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Utils;

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
  public String Username { get; set; } = "";
  [BindProperty]
  public String Password { get; set; } = "";

  public async Task<IActionResult> OnPostAsync()
  {
    var user = await _context.Users.FirstOrDefaultAsync(a => a.Email == Username || a.Username == Username);
    if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
    {
      ModelState.AddModelError("Username", "Invalid username or password");
      return Page();
    }

    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname),
      new Claim(ClaimTypes.Email, user.Email),
      new Claim(ClaimTypes.Role, "Client")
    };

    var claimsIdentity = new ClaimsIdentity(claims, "ClientAuth");
    var authProperties = new AuthenticationProperties { IsPersistent = true };

    await HttpContext.SignInAsync("ClientAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

    return RedirectToPage("/Client/Index");
  }

}