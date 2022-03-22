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

/* MsCustomParams.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Avalonia.Media.Imaging;

using AM.Avalonia.Enums;
using AM.Avalonia.Models;

#endregion

#nullable enable

namespace AM.Avalonia.DTO;

/// <summary>
///
/// </summary>
public class MsCustomParams
    : AbstractMessageBoxParams
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public MsCustomParams
        (
            MessageBoxCustomParams parameters
        )
    {
        Icon = parameters.Icon;
        ButtonDefinitions = parameters.ButtonDefinitions;
        UpdateLocal(parameters);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MsCustomParams
        (
            MessageBoxCustomParamsWithImage parameters
        )
    {
        BitmapIcon = parameters.Icon;
        ButtonDefinitions = parameters.ButtonDefinitions;
        UpdateLocal(parameters);
    }

    /// <summary>
    /// Messagebox icon
    /// </summary>
    public Icon Icon { get; set; } = Icon.None;

    /// <summary>
    /// Messagebox image
    /// </summary>
    public Bitmap? BitmapIcon { get; set; }

    /// <summary>
    /// Buttons
    /// </summary>
    public IEnumerable<ButtonDefinition> ButtonDefinitions { get; set; }

    private void UpdateLocal
        (
            AbstractMessageBoxParams parameters
        )
    {
        WindowIcon = parameters.WindowIcon;
        CanResize = parameters.CanResize;
        ShowInCenter = parameters.ShowInCenter;
        FontFamily = parameters.FontFamily;
        ContentTitle = parameters.ContentTitle;
        ContentHeader = parameters.ContentHeader;
        ContentMessage = parameters.ContentMessage;
        Markdown = parameters.Markdown;
        MinWidth = parameters.MinWidth;
        MaxWidth = parameters.MaxWidth;
        Width = parameters.Width;
        MinHeight = parameters.MinHeight;
        MaxHeight = parameters.MaxHeight;
        Height = parameters.Height;
        SizeToContent = parameters.SizeToContent;
        WindowStartupLocation = parameters.WindowStartupLocation;
        SystemDecorations = parameters.SystemDecorations;
        Topmost = parameters.Topmost;
    }
}
