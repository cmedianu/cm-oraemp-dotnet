using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OraEmp.WebUI.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty] public InputModel Input { get; set; }

    [FromQuery(Name = "returnUrl")]
    public string? returnUrl { get; set; }

    // public LoginModel(SignInManager<IdentityUser> signInManager)
    public LoginModel()
    {

    }
    public class InputModel
    {
        [Required]
        public string LoginName { get; set; }

        /*[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }*/
    }

    public  async Task<IActionResult> OnPostAsync()
    {

        if (ModelState.IsValid)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,Input.LoginName.ToUpper()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,Input.LoginName));
            claims.Add(new Claim("city","Vancouver"));
            claims.Add(new Claim(ClaimTypes.Role,"USER"));
            claims.Add(new Claim(ClaimTypes.Role,"ADMINISTRATOR"));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            // return LocalRedirect(ReturnUrl);
            return LocalRedirect("~/" + returnUrl);
        }

        return Page();
    }

    public void OnGet()
    {

    }


}