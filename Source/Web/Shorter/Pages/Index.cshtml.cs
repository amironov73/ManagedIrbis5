using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shorter.Pages;

public class IndexModel : PageModel
{
    public string? ShortLink { get; set; }
    private readonly ILogger<IndexModel> _logger;

    public IndexModel (ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet (string? go)
    {
        ShortLink = go;
    }
}
