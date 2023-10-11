using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OraEmp.Application.Common;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;

namespace OraEmp.WebUI.Areas.Identity.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private IDbSessionManagement _dbSessionManagement;

    [BindProperty] public InputModel Input { get; set; } = new InputModel();

    [FromQuery(Name = "returnUrl")]
    public string? returnUrl { get; set; }

    public LoginModel(IDbSessionManagement dbSessionManagement)
    {
        _dbSessionManagement = dbSessionManagement;
    }
    public class InputModel
    {
        [Required(ErrorMessage = "Login name is required.")]
        public string LoginName { get; set; }

        /*[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }*/
    }

    public  async Task<IActionResult> OnPostAsync()
    {
        
        if (ModelState.IsValid)
        {
            var loginName = Input.LoginName.ToLower();
            var roles =  _dbSessionManagement.GetRolesForUser(loginName);
            
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name,loginName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier,loginName));
            claims.Add(new Claim("city","Vancouver"));
            claims.Add(new Claim(IConstants.Ip, HttpContext.Connection.RemoteIpAddress.ToString()));
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return LocalRedirect("~/" + returnUrl);
        }
        return Page();
    }

    public void OnGet()
    {

    }
}