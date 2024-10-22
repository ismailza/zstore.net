using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using zstore.net.Services.Cart;

namespace zstore.net.Pages.Shop;

public class IndexModel : PageModel
{
  private readonly ILogger<IndexModel> _logger;
  private readonly ZStoreDbContext _context;
  private readonly ICartService _cartService;

  public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context, ICartService cartService)
  {
    _logger = logger;
    _context = context;
    _cartService = cartService;
  }

  public List<Product> Products { get; set; } = [];
  public int CurrentPage { get; set; } = 1;
  public int TotalPages { get; set; }
  public int PageSize { get; set; } = 12;

  [BindProperty(SupportsGet = true)]
  public string? SearchTerm { get; set; }

  [BindProperty(SupportsGet = true)]
  public long? CategoryId { get; set; }

  [BindProperty(SupportsGet = true)]
  public string? SortOrder { get; set; }

  public async Task OnGetAsync([FromQuery] int? page, [FromQuery] string? search, [FromQuery] long? category, [FromQuery] string? sort)
  {
    CurrentPage = page ?? 1;
    SearchTerm = search ?? SearchTerm;
    CategoryId = category ?? CategoryId;
    SortOrder = sort ?? SortOrder;

    var query = _context.Products.AsQueryable();
    // Apply search filter
    if (!string.IsNullOrEmpty(SearchTerm))
    {
      query = query.Where(p => p.Name.ToLower().Contains(SearchTerm.ToLower()));
    }
    // Apply category filter
    if (CategoryId.HasValue)
    {
      query = query.Where(p => p.CategoryId == CategoryId.Value);
    }
    // Apply sorting
    query = SortOrder switch
    {
      "name_asc" => query.OrderBy(p => p.Name),
      "name_desc" => query.OrderByDescending(p => p.Name),
      "price_asc" => query.OrderBy(p => p.Price),
      "price_desc" => query.OrderByDescending(p => p.Price),
      _ => query.OrderByDescending(p => p.UpdatedAt), // Default sort
    };

    // Get total count for pagination
    var totalProducts = await query.CountAsync();
    TotalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

    // Fetch products for the current page
    Products = await query
        .Skip((CurrentPage - 1) * PageSize)
        .Take(PageSize)
        .Include(p => p.Category)
        .ToListAsync();

    ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
  }

  public IActionResult OnPostAddToCart(long productId, int quantity = 1)
  {
    _cartService.AddToCart(productId, quantity);
    // Redirect to the cart page
    return RedirectToPage("/Cart/Index");
  }
}