using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Categories;

public class EditModel : AdminPageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Category Category { get; set; } = default!;

    public EditModel(ILogger<EditModel> logger, ZStoreDbContext context, IStorageService storageService)
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
        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Categories == null || Category == null)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Category.Id);
        if (category == null)
        {
            return NotFound();
        }

        try
        {
            if (Category.Image != null) {
                // Delete the existing image
                if (!string.IsNullOrEmpty(Category.ImageUri))
                {
                    await _storageService.DeleteAsync(Category.ImageUri);
                }
                Category.ImageUri = await _storageService.UploadAsync(Category.Image, "images/categories");
            } else {
                Category.ImageUri = category.ImageUri;
            }
            
            if (Category.Name != category.Name)
            {
                // Fetch all existing categories except the current one
                var existingCategories = await _context.Categories.Where(c => c.Id != Category.Id).ToListAsync();
                Category.GenerateSlug(existingCategories);
            } else {
                Category.Slug = category.Slug;
            }

            // Attach the category to the context and mark it as modified
            _context.Attach(Category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the category.");
            ModelState.AddModelError("", "An error occurred while updating the category. Please try again.");
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }
    }
}
