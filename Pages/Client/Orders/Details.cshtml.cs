using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Client.Orders;

public class DetailsModel : ClientPageModel
{
  private readonly ILogger<DetailsModel> _logger;
  private readonly ZStoreDbContext _context;

  public DetailsModel(ILogger<DetailsModel> logger, ZStoreDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  public Order Order { get; set; } = default!;

  public async Task<IActionResult> OnGetAsync(long id)
  {
    // Check if the user is authenticated
    if (User.Identity is not { IsAuthenticated: true })
    {
      _logger.LogWarning("Unauthorized access attempt to order details.");
      return RedirectToPage("/Client/Auth/Login");
    }

    // Get the client ID from claims
    if (!long.TryParse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out long clientId) || clientId == 0)
    {
      _logger.LogWarning("Client ID not found in claims or invalid.");
      return RedirectToPage("/Client/Auth/Login");
    }
    
    // Fetch the order details from the database
    Order = await _context.Orders
      .Include(o => o.OrderItems)
      .ThenInclude(oi => oi.Product)
      .FirstOrDefaultAsync(o => o.Id == id && o.UserId == clientId);

    if (Order == null)
    {
      _logger.LogWarning("Order with ID {OrderId} not found for client {ClientId}.", id, clientId);
      return RedirectToPage("/Client/Orders/Index");
    }

    return Page();
  }
}
