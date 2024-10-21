using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Admins;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Models.Admin Admin { get; set; } = default!;

    public DeleteModel(ILogger<DeleteModel> logger, ZStoreDbContext context, IStorageService storageService)
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
        if (Admin == null || _context.Admins == null)
        {
            return NotFound();
        }

        _context.Admins.Remove(Admin);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }


}
