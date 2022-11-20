// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MainActivity.cs -- главная активность приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Android.App;
using Android.Content.PM;

using Avalonia.Android;

#endregion

#nullable enable

namespace AvaloniaDroid;

/// <summary>
/// Главная активность приложения.
/// </summary>
[Activity(Label = "AvaloniaDroid", Theme = "@style/MyTheme.NoActionBar", 
  Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, 
  ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity 
    : AvaloniaMainActivity
{
    // пустое тело класса
}
