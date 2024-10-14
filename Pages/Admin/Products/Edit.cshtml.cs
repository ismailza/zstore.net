using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Products;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Product Product { get; set; } = default!;

    public EditModel(ILogger<EditModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var product =  await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        Product = product;
        ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Products == null || Product == null)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Product.Id);
        if (product == null)
        {
            return NotFound();
        }

        try
        {
            if (Product.Image != null) {
                // Delete the existing image
                if (!string.IsNullOrEmpty(Product.ImageUri))
                {
                    await _storageService.DeleteAsync(Product.ImageUri);
                }
                Product.ImageUri = await _storageService.UploadAsync(Product.Image, "images/products");
            } else {
                Product.ImageUri = product.ImageUri;
            }
            
            if (Product.Name != product.Name)
            {
                // Fetch all existing products except the current one
                var existingProducts = await _context.Products.Where(p => p.Id != Product.Id).ToListAsync();
                Product.GenerateSlug(existingProducts);
            } else {
                Product.Slug = Product.Slug;
            }

            Product.CreatedAt = product.CreatedAt;
            Product.UpdatedAt = DateTime.Now;

            // Attach the product to the context and mark it as modified
            _context.Attach(Product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the product.");
            ModelState.AddModelError("", "An error occurred while updating the product. Please try again.");
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }
    }
}
