using Microsoft.AspNetCore.Mvc;

using zstore.net.Models;
using zstore.net.Data;
using zstore.net.Services.Storage;

namespace zstore.net.Pages.Admin.Clients;

public class CreateModel : AdminPageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly ZStoreDbContext _context;
    private readonly IStorageService _storageService;
    
    [BindProperty]
    public User Client { get; set; } = default!;

    public CreateModel(ILogger<CreateModel> logger, ZStoreDbContext context, IStorageService storageService)
    {
        _logger = logger;
        _context = context;
        _storageService = storageService;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Users == null || Client == null)
        {
            _logger.LogError("Model validation failed. Errors: {Errors}", ModelState.Values.SelectMany(v => v.Errors));
            return Page();
        }

        try
        {
            String password = Utils.Utils.GetRandomString(8);
            _logger.LogInformation("Generated password: {Password}", password);
            Client.Password = Utils.Utils.GetHash(password);
            
            _context.Users.Add(Client);
            await _context.SaveChangesAsync();

            // TODO: Send email to the client with the password

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the client.");
            ModelState.AddModelError("", "An error occurred while creating the client. Please try again.");
            return Page();
        }
    }

}
