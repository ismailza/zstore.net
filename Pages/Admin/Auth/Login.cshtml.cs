using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Utils;

namespace zstore.net.Pages.Admin.Auth;

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
  [EmailAddress]
  public String Username { get; set; } = "";
  [BindProperty]
  public String Password { get; set; } = "";

  [BindProperty(SupportsGet = true)]
  public string? ReturnUrl { get; set; }

  public async Task<IActionResult> OnPostAsync()
  {
    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == Username);
    if (admin == null || !Utils.Utils.VerifyHash(Password, admin.Password))
    {
      ModelState.AddModelError("Username", "Invalid username or password");
      return Page();
    }

    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, admin.Firstname + " " + admin.Lastname),
      new Claim(ClaimTypes.Email, admin.Email),
      new Claim(ClaimTypes.Role, "Admin")
    };

    var claimsIdentity = new ClaimsIdentity(claims, "AdminAuth");
    var authProperties = new AuthenticationProperties { IsPersistent = true };

    await HttpContext.SignInAsync("AdminAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
    {
      return LocalRedirect(ReturnUrl);
    }
    return RedirectToPage("/Admin/Index");
  }

}