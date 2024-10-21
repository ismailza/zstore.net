using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Categories;

public class CreateModel : AdminPageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;
    
    [BindProperty]
    public Category Category { get; set; } = default!;

    public CreateModel(ILogger<CreateModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public IActionResult OnGet()
    {
        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Categories == null || Category == null || Category.Image == null)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            _logger.LogError("Model validation failed. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return Page();
        }

        try
        {
            Category.ImageUri = await _storageService.UploadAsync(Category.Image, "images/categories");

            // Fetch all existing categories from the database
            var existingCategories = await _context.Categories.ToListAsync();
            Category.GenerateSlug(existingCategories);

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the category.");
            ModelState.AddModelError("", "An error occurred while creating the category. Please try again.");
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }
    }

}
