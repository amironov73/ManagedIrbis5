// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace WebPlace;

internal sealed class Program
{
    public static int Main
        (
            string[] args
        )
    {
        var builder = WebApplication.CreateBuilder (args);

        builder.Services.AddRazorPages();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler ("/Error");
        }

        var config = app.Services.GetRequiredService<IConfiguration>();
        var pathBase = config["path-base"];
        if (!string.IsNullOrEmpty (pathBase))
        {
            // приложение не в корне
            app.UsePathBase (pathBase);
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation ("Path-base is {PathBase}", pathBase);
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.MapRazorPages();

        app.Run();

        return 0;
    }
}
