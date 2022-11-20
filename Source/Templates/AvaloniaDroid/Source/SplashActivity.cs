// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* SplashActivity.cs -- заставка приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Android.App;
using Android.Content;
using Android.OS;

using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

using Application = Android.App.Application;

#endregion

#nullable enable

namespace AvaloniaDroid;

/// <summary>
/// Заставка приложения.
/// </summary>
[Activity (Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
internal sealed class SplashActivity 
   : AvaloniaSplashActivity<App>
{
    #region AvaloniaSplashActivity members

    /// <inheritdoc cref="AvaloniaSplashActivity{TApp}.CustomizeAppBuilder"/>
    protected override AppBuilder CustomizeAppBuilder
        (
            AppBuilder builder
        )
    {
        return base.CustomizeAppBuilder (builder)
            .UseReactiveUI();
    }

    /// <inheritdoc cref="AvaloniaSplashActivity.OnCreate(Android.OS.Bundle?)"/>
    protected override void OnCreate
        (
            Bundle? savedInstanceState
        )
    {
        base.OnCreate (savedInstanceState);
    }

    #endregion

    #region Activity members

    /// <inheritdoc cref="Activity.OnResume"/>
    protected override void OnResume()
    {
        base.OnResume();

        StartActivity
            (
                new Intent (Application.Context, typeof (MainActivity))
            );
    }

    #endregion
}
