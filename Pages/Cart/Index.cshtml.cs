using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using zstore.net.Data;
using zstore.net.Models;
using zstore.net.Services.Cart;

namespace zstore.net.Pages.Cart;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly ICartService _cartService;

    public Dictionary<Product, int> cartItems { get; set; } = new Dictionary<Product, int>();

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context, ICartService cartService)
    {
        _logger = logger;
        _context = context;
        _cartService = cartService;
    }

    public void OnGet()
    {
        var cart = _cartService.GetCartItems();
        foreach (var (productId, quantity) in cart)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                cartItems.Add(product, quantity);
            }
        }
    }

    public IActionResult OnPostRemoveFromCart(long productId)
    {
        _cartService.RemoveFromCart(productId);
        return RedirectToPage();
    }

    public IActionResult OnPostUpdateQuantity(long productId, int quantity)
    {
        _cartService.AddToCart(productId, quantity);
        return RedirectToPage();
    }

    public IActionResult OnPostClearCart()
    {
        _cartService.ClearCart();
        return RedirectToPage();
    }

}
