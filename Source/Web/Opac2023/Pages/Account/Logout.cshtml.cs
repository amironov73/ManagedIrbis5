using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Opac2023.Pages.Account;

public class LogoutModel: PageModel
{
    public Task<IActionResult> OnGetAsync()
    {
        return TakeAction();
    }

    public Task<IActionResult> OnPostAsync()
    {
        return TakeAction();
    }

    private async Task<IActionResult> TakeAction()
    {
        await HttpContext.SignOutAsync(Constants.AuthenticationScheme);

        return RedirectToPage("/Index");
    }
}