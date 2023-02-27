using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Opac2023.Pages.Account;

public class LoginModel: PageModel
{
    [BindProperty]
    public OpacCredential? Credential { get; set; }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var claims = new List<Claim>()
        {
            new (ClaimTypes.Name, Credential!.UserName!)
        };
        var identity = new ClaimsIdentity (claims, Constants.AuthenticationScheme);
        var principal = new ClaimsPrincipal (identity);
        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = Credential!.RememberMe
        };
        await HttpContext.SignInAsync (Constants.AuthenticationScheme, principal, authProperties);

        return RedirectToPage ("/Index");
    }
}
