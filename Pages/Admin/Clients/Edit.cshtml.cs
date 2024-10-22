using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Clients;

public class EditModel : AdminPageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;

    [BindProperty]
    public User Client { get; set; } = default!;

    public EditModel(ILogger<EditModel> logger, ZStoreDbContext context, IStorageService storageService)
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
        if (!ModelState.IsValid || _context.Users == null || Client == null)
        {
            return Page();
        }

        var client = await _context.Users.AsNoTracking().FirstOrDefaultAsync(m => m.Id == Client.Id);
        if (client == null)
        {
            return NotFound();
        }

        try
        {
            Client.Password = client.Password;
            Client.CreatedAt = client.CreatedAt;
            Client.UpdatedAt = DateTime.Now;
            _context.Attach(Client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the client.");
            ModelState.AddModelError("", "An error occurred while updating the client. Please try again.");
            return Page();
        }
    }
}
