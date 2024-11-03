using Microsoft.AspNetCore.Mvc;

namespace zstore.net.Controllers.Client.Auth;

public class AuthController : Controller
{
  [HttpGet]
  public IActionResult Index()
  {
    // Check if the user is authenticated
    if (User.Identity is { IsAuthenticated: true })
    {
      return LocalRedirect("/Client/Index");
    }
    return View("Client/Auth/Login");
  }

  [HttpPost]
  public IActionResult Login(string username, string password, [FromQuery] string redirectUrl)
  {
    // Check if the user is not authenticated
    if (User.Identity is not { IsAuthenticated: true })
    {
      // TODO: Implement the login logic
    }
    return LocalRedirect("/Client/Index");
  }
  
}