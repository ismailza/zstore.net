using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Dictionary<Category, List<Category>> CategoriesGroupedByParent { get; set; } = [];

    public async Task OnGetAsync()
    {
        // Fetch all categories including their parent category information
        var categories = await _context.Categories
                                       .Include(c => c.ParentCategory)
                                       .ToListAsync();

        // Group categories by their parent category
        CategoriesGroupedByParent = categories
            .Where(c => c.ParentCategoryId != null)
            .GroupBy(c => c.ParentCategory)
            .ToDictionary(g => g.Key!, g => g.ToList());

        // For parentless categories (root categories)
        var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

        // Add root categories with empty subcategory lists to the dictionary
        foreach (var root in rootCategories)
        {
            if (!CategoriesGroupedByParent.ContainsKey(root))
            {
                CategoriesGroupedByParent.Add(root, []);
            }
        }
    }

}
