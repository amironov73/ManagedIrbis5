// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* MessageBoxManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

using AM.Avalonia.BaseWindows.Base;
using AM.Avalonia.DTO;
using AM.Avalonia.Enums;
using AM.Avalonia.ViewModels;

using CustomWindow = AM.Avalonia.Views.MsBoxCustomWindow;
using HyperlinkWindow = AM.Avalonia.Views.MsBoxHyperlinkWindow;
using InputWindow = AM.Avalonia.Views.MsBoxInputWindow;
using StandardWindow = AM.Avalonia.Views.MsBoxStandardWindow;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
///
/// </summary>
public static class MessageBoxManager
{
    /// <summary>
    /// Create instance of custom messagebox window
    /// </summary>
    /// <param name="params"> Params for custom window </param>
    /// <returns></returns>
    public static IMsBoxWindow<string> GetMessageBoxCustomWindow (MessageBoxCustomParams @params)
    {
        var window = new CustomWindow();
        window.DataContext = new MsBoxCustomViewModel (new MsCustomParams (@params), window);
        return new MsBoxWindowBase<CustomWindow, string> (window);
    }

    /// <summary>
    /// Create instance of custom messagebox window (with image instead of default icon)
    /// </summary>
    /// <param name="params"> Params for custom window </param>
    /// <returns></returns>
    public static IMsBoxWindow<string> GetMessageBoxCustomWindow (MessageBoxCustomParamsWithImage @params)
    {
        var window = new CustomWindow();
        window.DataContext = new MsBoxCustomViewModel (new MsCustomParams (@params), window);
        return new MsBoxWindowBase<CustomWindow, string> (window);
    }

    /// <summary>
    /// Create instance of standard messagebox window
    /// </summary>
    /// <param name="params">Params for standard window</param>
    /// <returns></returns>
    public static IMsBoxWindow<ButtonResult> GetMessageBoxStandardWindow (MessageBoxStandardParams @params)
    {
        var window = new StandardWindow();
        window.DataContext = new MsBoxStandardViewModel (@params, window);
        return new MsBoxWindowBase<StandardWindow, ButtonResult> (window);
    }

    /// <summary>
    /// Create instance of hyperlink messagebox window
    /// </summary>
    /// <param name="params">Params for hyperlink window</param>
    /// <returns></returns>
    public static IMsBoxWindow<ButtonResult> GetMessageBoxHyperlinkWindow (MessageBoxHyperlinkParams @params)
    {
        var window = new HyperlinkWindow();
        window.DataContext = new MsBoxHyperlinkViewModel (@params, window);
        return new MsBoxWindowBase<HyperlinkWindow, ButtonResult> (window);
    }

    /// <summary>
    /// Create instance of standard messagebox window
    /// </summary>
    /// <param name="title"> Windows title </param>
    /// <param name="text"> Text of messagebox body </param>
    /// <param name="enum"> Buttons of messagebox (default OK) </param>
    /// <param name="icon"> Icon of messagebox (default no icon) </param>
    /// <param name="windowStartupLocation"> Startup location of messagebox (default center screen) </param>
    /// <param name="style"></param>
    /// <returns></returns>
    /// <remarks>
    /// Recommended method for messadge box
    /// </remarks>
    public static IMsBoxWindow<ButtonResult> GetMessageBoxStandardWindow (string title, string text,
        ButtonEnum @enum = ButtonEnum.Ok, Icon icon = Icon.None,
        WindowStartupLocation windowStartupLocation = WindowStartupLocation.CenterScreen) =>
        GetMessageBoxStandardWindow (new MessageBoxStandardParams
        {
            ContentTitle = title,
            ContentMessage = text,
            ButtonDefinitions = @enum,
            Icon = icon,
            WindowStartupLocation = windowStartupLocation
        });

    /// <summary>
    /// Create instance of standard messagebox window
    /// </summary>
    /// <param name="params">Params for input window</param>
    /// <returns></returns>
    public static IMsBoxWindow<MessageWindowResultDTO> GetMessageBoxInputWindow (MessageBoxInputParams @params)
    {
        var window = new InputWindow();
        window.DataContext = new MsBoxInputViewModel (@params, window);
        return new MsBoxWindowBase<InputWindow, MessageWindowResultDTO> (window);
    }
}
