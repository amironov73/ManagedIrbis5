// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

#region Using directives

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Opac2023;

internal sealed class Program
{
    public static int Main
        (
            string[] args
        )
    {
        var builder = WebApplication.CreateBuilder (args);
        builder.Services.AddRazorPages();
        builder.Services.AddAuthentication (Constants.AuthenticationScheme)
            .AddCookie (Constants.AuthenticationScheme, options =>
            {
                options.Cookie.Name = Constants.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromHours (1);
            });

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

        app.UseForwardedHeaders (new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor
                | ForwardedHeaders.XForwardedProto
        });

        app.UseAuthentication();
        app.UseAuthorization ();

        app.Run();

        return 0;
    }
}
