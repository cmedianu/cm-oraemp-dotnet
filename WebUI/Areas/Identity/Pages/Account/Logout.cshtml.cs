using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OraEmp.WebUI.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{
    [FromQuery(Name = "returnUrl")]
    public string? returnUrl { get; set; }
    public async Task<IActionResult> OnGet()
    {
        await HttpContext.SignOutAsync();
        return LocalRedirect("~/" + returnUrl);
    }
}