using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace zstore.net.Pages.Client;

public class IndexModel : ClientPageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string? ClientName { get; private set; } // Store the client's name

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        // Ensure the correct authentication scheme is being used
        var clientAuth = HttpContext.User.Identities
            .FirstOrDefault(identity => identity.AuthenticationType == "ClientAuth");

        if (clientAuth != null)
        {
            // Retrieve the client's name from the claims
            ClientName = clientAuth.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
