using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace zstore.net.Pages.Admin;

[Authorize(Roles = "Admin")]
public class AdminPageModel : PageModel
{
}