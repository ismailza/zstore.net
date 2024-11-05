using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Client.Account;

public class IndexModel : ClientPageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly ZStoreDbContext _context;

  public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  [BindProperty]
  public Models.User? Client { get; set; } = default!;

  [BindProperty]
  public string CurrentPassword { get; set; } = string.Empty;

  [BindProperty]
  public string NewPassword { get; set; } = string.Empty;

  [BindProperty]
  public string ConfirmPassword { get; set; } = string.Empty;

  public async Task<IActionResult> OnGetAsync()
  {
    // Check if the user is authenticated
    if (User.Identity is not { IsAuthenticated: true })
    {
      return RedirectToPage("/Client/Auth/Login");
    }

    // Get the client id from claims
    long clientId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

    if (clientId == 0)
    {
      // Handle the case where the id is not found in claims
      return RedirectToPage("/Client/Auth/Login");
    }

    // Fetch the client details from the database with address
    Client = await _context.Users
      .Include(c => c.Address)
      .FirstOrDefaultAsync(c => c.Id == clientId);

    if (Client == null)
    {
      // Handle the case where the client does not exist
      return RedirectToPage("/Client/Auth/Login");
    }

    return Page();
  }

  public async Task<IActionResult> OnPostUpdateInfoAsync()
  {
    _logger.LogInformation("Updating account information");
    if (Client == null)
    {
      return RedirectToPage();
    }

    var existingClient = await _context.Users.FirstOrDefaultAsync(a => a.Id == Client.Id);

    if (existingClient == null)
    {
      ModelState.AddModelError(string.Empty, "Client not found.");
      return RedirectToPage();
    }

    // Update client details
    existingClient.Firstname = Client.Firstname;
    existingClient.Lastname = Client.Lastname;
    existingClient.Email = Client.Email;

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Account information updated successfully.";

    _logger.LogInformation("Account information updated successfully");
    return RedirectToPage();
  }

  public async Task<IActionResult> OnPostUpdateAddressAsync()
  {
    _logger.LogInformation("Updating address information");
    if (Client == null)
    {
      return RedirectToPage();
    }

    var existingClient = await _context.Users
      .Include(c => c.Address)
      .FirstOrDefaultAsync(c => c.Id == Client.Id);

    if (existingClient == null)
    {
      ModelState.AddModelError(string.Empty, "Client not found.");
      return RedirectToPage();
    }

    if (existingClient.Address == null)
    {
      existingClient.Address = new Address
      {
        Street = Client!.Address!.Street,
        City = Client.Address.City,
        State = Client.Address.State,
        Zip = Client.Address.Zip,
        Country = Client.Address.Country
      };
    } else {
      existingClient.Address.Street = Client!.Address!.Street;
      existingClient.Address.City = Client.Address.City;
      existingClient.Address.State = Client.Address.State;
      existingClient.Address.Zip = Client.Address.Zip;
      existingClient.Address.Country = Client.Address.Country;
    }

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Address updated successfully.";

    _logger.LogInformation("Address updated successfully");
    return RedirectToPage();
  }

  public async Task<IActionResult> OnPostUpdatePasswordAsync()
  {
    _logger.LogInformation("Updating password");
    if (Client == null)
    {
      return RedirectToPage();
    }

    if (NewPassword != ConfirmPassword)
    {
      ModelState.AddModelError(string.Empty, "New password and confirmation do not match.");
      return RedirectToPage();
    }

    var existingClient = await _context.Users.FirstOrDefaultAsync(a => a.Id == Client.Id);

    if (existingClient == null)
    {
      ModelState.AddModelError(string.Empty, "Client not found.");
      return RedirectToPage();
    }

    // Verify current password
    if (!Utils.Utils.VerifyHash(CurrentPassword, existingClient.Password))
    {
      ModelState.AddModelError(string.Empty, "Current password is incorrect.");
      return RedirectToPage();
    }

    // Update password
    existingClient.Password = Utils.Utils.GetHash(NewPassword);

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Password updated successfully.";

    _logger.LogInformation("Password updated successfully");
    return RedirectToPage();
  }

}
