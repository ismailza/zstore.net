using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Client;

[Authorize(AuthenticationSchemes = "ClientAuth", Policy = "ClientOnly")]
public class ClientPageModel : PageModel
{
}