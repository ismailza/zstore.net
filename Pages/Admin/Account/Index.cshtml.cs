using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;

namespace zstore.net.Pages.Admin.Account;

public class IndexModel : AdminPageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly ZStoreDbContext _context;

  public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  [BindProperty]
  public Models.Admin? Admin { get; set; } = default!;

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
      return RedirectToPage("/Admin/Auth/Login");
    }

    // Get the admin email from claims
    var adminEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

    if (string.IsNullOrEmpty(adminEmail))
    {
      // Handle the case where the email is not found in claims
      return RedirectToPage("/Admin/Auth/Login");
    }

    // Fetch the admin details from the database
    Admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == adminEmail);

    if (Admin == null)
    {
      // Handle the case where the admin does not exist
      return RedirectToPage("/Admin/Auth/Login");
    }

    return Page();
  }

  public async Task<IActionResult> OnPostUpdateInfoAsync()
  {
    _logger.LogInformation("Updating account information");
    if (Admin == null)
    {
      return RedirectToPage();
    }

    var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == Admin.Id);

    if (existingAdmin == null)
    {
      ModelState.AddModelError(string.Empty, "Admin not found.");
      return RedirectToPage();
    }

    // Update admin details
    existingAdmin.Firstname = Admin.Firstname;
    existingAdmin.Lastname = Admin.Lastname;
    existingAdmin.Email = Admin.Email;

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Account information updated successfully.";

    _logger.LogInformation("Account information updated successfully");
    return RedirectToPage();
  }

  public async Task<IActionResult> OnPostUpdatePasswordAsync()
  {
    _logger.LogInformation("Updating password");
    if (Admin == null)
    {
      return RedirectToPage();
    }

    if (NewPassword != ConfirmPassword)
    {
      ModelState.AddModelError(string.Empty, "New password and confirmation do not match.");
      return RedirectToPage();
    }

    var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == Admin.Id);

    if (existingAdmin == null)
    {
      ModelState.AddModelError(string.Empty, "Admin not found.");
      return RedirectToPage();
    }

    // Verify current password
    if (!Utils.Utils.VerifyHash(CurrentPassword, existingAdmin.Password))
    {
      ModelState.AddModelError(string.Empty, "Current password is incorrect.");
      return RedirectToPage();
    }

    // Update password
    existingAdmin.Password = Utils.Utils.GetHash(NewPassword);

    await _context.SaveChangesAsync();
    TempData["SuccessMessage"] = "Password updated successfully.";

    _logger.LogInformation("Password updated successfully");
    return RedirectToPage();
  }

}
