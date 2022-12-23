// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FluentThemeResources.cs -- ресурсы темы Fluent.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
/// Ресурсы темы Fluent.
/// </summary>
public static class FluentThemeResources
{
    #region Цвета акцентов

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorDark1 (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorDark2 (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorDark3 (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorLight1 (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorLight2 (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAccentColorLight3 (this IResourceHost host) => host.FindColor();

    #endregion

    #region Системные цвета

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAltHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAltLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAltMediumColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAltMediumHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemAltMediumLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemBaseHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemBaseLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemBaseMediumColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemBaseMediumHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemBaseMediumLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeAltLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeBlackHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeBlackLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeBlackMediumLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeBlackMediumColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeDisabledHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeDisabledLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeHighColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeMediumColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeMediumLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeWhiteColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemChromeGrayColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemListLowColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemListMediumColor (this IResourceHost host) => host.FindColor();

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static Color SystemErrorTextColor (this IResourceHost host) => host.FindColor();

    #endregion

    #region Кисти для системных акцентов

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrush (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColor (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushDark1 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorDark1 (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushDark2 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorDark2 (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushDark3 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorDark3 (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushLight1 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorLight1 (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushLight2 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorLight2 (host));

    /// <summary>
    ///
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static IBrush SystemAccentBrushLight3 (this IResourceHost host)
        => new SolidColorBrush (SystemAccentColorLight3 (host));

    #endregion
}
