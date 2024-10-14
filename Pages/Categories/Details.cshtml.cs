using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;
using zstore.net.Services.Cart;

namespace zstore.net.Pages.Categories;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly ICartService _cartService;

    public DetailsModel(ILogger<DetailsModel> logger, ZStoreDbContext context, ICartService cartService)
    {
        _logger = logger;
        _context = context;
        _cartService = cartService;
    }

    public Category? Category { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        Category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(m => m.Slug != null && m.Slug == slug);

        if (Category == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPostAddToCart(long productId, int quantity = 1)
    {
        _cartService.AddToCart(productId, quantity);
        // Redirect to the cart page
        return RedirectToPage("/Cart/Index");
    }

}
