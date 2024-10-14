using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Category> Categories { get; set; } = [];

    public async Task OnGetAsync()
    {
        Categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
