using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Admin.Products;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Product> Products { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        var totalProducts = await _context.Products.CountAsync();
        TotalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);
        
        // Fetch products for the current page
        Products = await _context.Products
            .OrderByDescending(p => p.UpdatedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .Include(p => p.Category)
            .ToListAsync();
    }
}
