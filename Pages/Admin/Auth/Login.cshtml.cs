using System.ComponentModel.DataAnnotations;
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

  public async Task<IActionResult> OnPostAsync()
  {
    var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == Username);
    if (admin == null)
    {
      ModelState.AddModelError("Username", "Invalid username or password");
      return Page();
    }

    if (!Utils.Utils.VerifyHash(Password, admin.Password))
    {
      ModelState.AddModelError("Username", "Invalid username or password");
      return Page();
    }

    HttpContext.Session.SetString("Admin", admin.Email);
    return RedirectToPage("/Admin/Index");
  }

}