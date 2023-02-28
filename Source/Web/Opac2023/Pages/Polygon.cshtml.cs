// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable IdentifierTypo

#region Using directives

using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

#nullable enable

namespace Opac2023.Pages;

public class Polygon
    : PageModel
{
    public Polygon (ILogger<Polygon> logger)
    {
        _logger = logger;
    }

    private readonly ILogger _logger;

    public void OnGet()
    {
        _logger.LogInformation ("GET Polygon");
    }
}
