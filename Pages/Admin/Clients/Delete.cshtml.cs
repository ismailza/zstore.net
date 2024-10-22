using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Data;
using zstore.net.Models;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Clients;

public class DeleteModel : AdminPageModel
{
    private readonly ILogger<DeleteModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public User Client { get; set; } = default!;

    public DeleteModel(ILogger<DeleteModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public async Task<IActionResult> OnGetAsync(long? id)
    {
        if (id == null || _context.Users == null)
        {
            return NotFound();
        }

        var client =  await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        if (client == null)
        {
            return NotFound();
        }
        Client = client;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Client == null || _context.Users == null)
        {
            return NotFound();
        }

        _context.Users.Remove(Client);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }


}
