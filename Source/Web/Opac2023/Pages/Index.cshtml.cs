// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

#nullable enable

namespace Opac2023.Pages;

public class Index : PageModel
{
    public Index (ILogger<Index> logger)
    {
        _logger = logger;
    }

    private readonly ILogger _logger;
    
    public void OnGet()
    {
        _logger.LogInformation ("GET Index");
    }
}
