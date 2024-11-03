using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Client.Orders;

public class IndexModel : ClientPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Order> Orders { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        var totalOrders = await _context.Orders.CountAsync();
        TotalPages = (int)Math.Ceiling(totalOrders / (double)PageSize);

        // Get the user ID from the claims
        long userId = long.Parse(User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        
        // Fetch orders for the current page
        Orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .Include(o => o.Address)
            .ToListAsync();
    }
}
