using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Categories;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly ZStoreDbContext _context;

    public DetailsModel(ILogger<DetailsModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
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

}
