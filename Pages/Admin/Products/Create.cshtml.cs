using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Products;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;
    
    [BindProperty]
    public Product Product { get; set; } = default!;

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
        if (!ModelState.IsValid || _context.Categories == null || Product == null || Product.Image == null)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            _logger.LogError("Model validation failed. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return Page();
        }

        try
        {
            Product.ImageUri = await _storageService.UploadAsync(Product.Image, "images/products");

            // Fetch all existing products from the database
            var existingProducts = await _context.Products.ToListAsync();
            Product.GenerateSlug(existingProducts);

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the product.");
            ModelState.AddModelError("", "An error occurred while creating the product. Please try again.");
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }
    }

}
