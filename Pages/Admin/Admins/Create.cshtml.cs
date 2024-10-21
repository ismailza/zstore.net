using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Admins;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;
    
    [BindProperty]
    public Models.Admin Admin { get; set; } = default!;

    public CreateModel(ILogger<CreateModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Admins == null || Admin == null)
        {
            _logger.LogError("Model validation failed. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return Page();
        }

        try
        {
            String password = Utils.Utils.GetRandomString(8);
            _logger.LogInformation("Generated password: {Password}", password);
            Admin.Password = Utils.Utils.GetHash(password);
            
            _context.Admins.Add(Admin);
            await _context.SaveChangesAsync();

            // TODO: Send email to the admin with the password

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the admin.");
            ModelState.AddModelError("", "An error occurred while creating the admin. Please try again.");
            return Page();
        }
    }

}
