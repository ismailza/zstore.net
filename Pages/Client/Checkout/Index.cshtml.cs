using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Enums;
using zstore.net.Models;
using zstore.net.Services.Cart;

namespace zstore.net.Pages.Client.Checkout;

public class IndexModel : ClientPageModel
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

  public Dictionary<Product, int> CartItems { get; set; } = new Dictionary<Product, int>();
  [BindProperty]
  public User? Client { get; set; } = default!;
  [BindProperty]
  public Order Order { get; set; } = default!;

  public async Task<IActionResult> OnGetAsync()
  {
    // Check if the user is authenticated
    if (User.Identity is not { IsAuthenticated: true })
    {
      return RedirectToPage("/Client/Auth/Login");
    }
    // Get the client id from claims
    long clientId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
    if (clientId == 0)
    {
      // Handle the case where the id is not found in claims
      return RedirectToPage("/Client/Auth/Login");
    }
    // Fetch the client details from the database with address
    Client = await _context.Users
      .Include(c => c.Address)
      .FirstOrDefaultAsync(c => c.Id == clientId);
    if (Client == null)
    {
      // Handle the case where the client does not exist
      return RedirectToPage("/Client/Auth/Login");
    }
    return Page();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    _logger.LogInformation("Order submitted");

    // Repopulate CartItems from the cart service
    var cart = _cartService.GetCartItems();
    foreach (var (productId, quantity) in cart)
    {
      var product = _context.Products.Find(productId);
      if (product != null)
      {
        CartItems.Add(product, quantity);
      }
    }

    Order.UserId = Client!.Id;
    Order.Status = OrderStatus.PENDING;
    Order.OrderItems = new List<OrderItem>();

    Order = _context.Orders.Add(Order).Entity;
    await _context.SaveChangesAsync();

    foreach (var (product, quantity) in CartItems)
    {
      _context.OrderItems.Add(new OrderItem
      {
        OrderId = Order.Id,
        ProductId = product.Id,
        Quantity = quantity,
        Price = product.Price,
      });
    }
    await _context.SaveChangesAsync();

    _cartService.ClearCart();
    return RedirectToPage("/Client/Orders/Details", new { id = Order.Id });
  }


}