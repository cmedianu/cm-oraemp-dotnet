using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OraEmp.WebUI.Areas.Identity.Pages.Account;

public class LogoutModel : PageModel
{





    public async Task OnGet()
    {

        await HttpContext.SignOutAsync();
        string returnUrl = Url.Content("~/");
        LocalRedirect(returnUrl);

    }


}