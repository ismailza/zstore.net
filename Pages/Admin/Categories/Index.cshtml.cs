using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Admin.Categories;

public class IndexModel : AdminPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Category> Categories { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        var totalCategories = await _context.Categories.CountAsync();
        TotalPages = (int)Math.Ceiling(totalCategories / (double)PageSize);
        
        // Fetch categories for the current page
        Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
