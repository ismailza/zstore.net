using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Client;

[Authorize(Roles = "Client")]
public class ClientPageModel : PageModel
{
}