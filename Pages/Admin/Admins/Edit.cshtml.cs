using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Admins;

public class EditModel : AdminPageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Models.Admin Admin { get; set; } = default!;

    public EditModel(ILogger<EditModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null || _context.Admins == null)
        {
            return NotFound();
        }

        var admin =  await _context.Admins.FirstOrDefaultAsync(m => m.Id == id);
        if (admin == null)
        {
            return NotFound();
        }
        Admin = admin;
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Admins == null || Admin == null)
        {
            return Page();
        }

        var admin = await _context.Admins.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Admin.Id);
        if (admin == null)
        {
            return NotFound();
        }

        try
        {
            Admin.Password = admin.Password;
            Admin.CreatedAt = admin.CreatedAt;
            Admin.UpdatedAt = DateTime.Now;
            _context.Attach(Admin).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the admin.");
            ModelState.AddModelError("", "An error occurred while updating the admin. Please try again.");
            return Page();
        }
    }
}
