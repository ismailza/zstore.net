using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Client.Auth;

public class LogoutModel : PageModel
{
  private readonly ILogger<LogoutModel> _logger;

  public LogoutModel(ILogger<LogoutModel> logger)
  {
    _logger = logger;
  }

  public async Task<IActionResult> OnPostAsync()
  {
    _logger.LogInformation("Client user logging out.");
    await HttpContext.SignOutAsync("ClientAuth");
    HttpContext.Session.Clear();
    return RedirectToPage("/Client/Auth/Login");
  }

}
