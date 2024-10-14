using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;
using zstore.net.Services.Cart;

namespace zstore.net.Pages.Products;

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

  public Product? Product { get; set; }
  public List<Product> RelatedProducts { get; set; } = [];

  public async Task<IActionResult> OnGetAsync(string slug)
  {
    Product = await _context.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(m => m.Slug != null && m.Slug == slug);

    if (Product == null)
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