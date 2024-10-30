using Microsoft.AspNetCore.Mvc;
using zstore.net.Models;

namespace zstore.net.Controllers.Client.Auth;

public class RegisterController : Controller
{
  [HttpGet]
  public IActionResult Index()
  {
    // Check if the user is authenticated
    if (User.Identity is { IsAuthenticated: true })
    {
      return LocalRedirect("/Client/Index");
    }
    return View("Client/Auth/Register");
  }

  [HttpPost]
  public IActionResult Register(User user, [FromQuery] string redirectUrl)
  {
    if (user == null)
    {
      return BadRequest();
    }
    // Check if the user is not authenticated
    if (User.Identity is not { IsAuthenticated: true })
    {
      // TODO: Implement the register logic
    }
    return LocalRedirect("/Client/Index");
  }
  
}