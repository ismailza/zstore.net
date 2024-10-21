using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using zstore.net.Data;
using zstore.net.Models;

namespace zstore.net.Pages.Admin.Admins;

public class IndexModel : AdminPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ZStoreDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ZStoreDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Models.Admin> Admins { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync([FromQuery] int? page)
    {
        CurrentPage = page ?? 1;
        var totalAdmins = await _context.Admins.CountAsync();
        TotalPages = (int)Math.Ceiling(totalAdmins / (double)PageSize);
        
        // Fetch admins for the current page
        Admins = await _context.Admins
            .OrderByDescending(c => c.CreatedAt)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
