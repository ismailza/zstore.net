using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Admin;

[Authorize(AuthenticationSchemes = "AdminAuth", Policy = "AdminOnly")]
public class AdminPageModel : PageModel
{
}