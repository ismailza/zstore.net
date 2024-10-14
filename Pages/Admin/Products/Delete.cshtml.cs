using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Products;

public class DeleteModel : PageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public Product Product { get; set; } = default!;

    public DeleteModel(ILogger<DeleteModel> logger, ZStoreDbContext context, IStorageService storageService)
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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Product == null || _context.Products == null)
        {
            return NotFound();
        }

        if (Product.ImageUri != null)
        {
            await _storageService.DeleteAsync(Product.ImageUri);
        }

        _context.Products.Remove(Product);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }


}
