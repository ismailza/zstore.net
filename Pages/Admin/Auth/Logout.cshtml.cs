using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Admin.Auth;

public class LogoutModel : PageModel
{
  private readonly ILogger<LogoutModel> _logger;

  public LogoutModel(ILogger<LogoutModel> logger)
  {
    _logger = logger;
  }

  public async Task<IActionResult> OnPostAsync()
  {
    _logger.LogInformation("Admin user logging out.");
    await HttpContext.SignOutAsync("AdminAuth");
    HttpContext.Session.Clear();
    return RedirectToPage("/Admin/Auth/Login");
  }

}
