using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Categories;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Category Category { get; set; } = default!;

    public DeleteModel(ILogger<DeleteModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null || _context.Categories == null)
        {
            return NotFound();
        }

        var category =  await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        Category = category;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Category == null || _context.Categories == null)
        {
            return NotFound();
        }

        if (Category.ImageUri != null)
        {
            await _storageService.DeleteAsync(Category.ImageUri);
        }

        _context.Categories.Remove(Category);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }


}
