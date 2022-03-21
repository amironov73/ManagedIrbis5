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

/* MsBoxStandardWindow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using AM.Avalonia.BaseWindows.Base;
using AM.Avalonia.Enums;

#endregion

#nullable enable

namespace AM.Avalonia.Views;

/// <summary>
///
/// </summary>
public class MsBoxStandardWindow
    : BaseWindow, IWindowGetResult<ButtonResult>
{
    /// <summary>
    ///
    /// </summary>
    public MsBoxStandardWindow()
    {
        InitializeComponent();
    }

    public ButtonResult ButtonResult { get; set; } = ButtonResult.None;

    public ButtonResult GetResult() => ButtonResult;

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree (VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree (e);
        var okButton = this.FindControl<Button> ("OkButton");
        okButton.Focus();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load (this);
    }

    protected override void OnOpened (EventArgs e)
    {
        base.OnOpened (e);

        // Hack to fix scroll bar and limits
        if (SizeToContent != SizeToContent.Manual)
        {
            SizeToContent = SizeToContent.Manual;
            Width--;
            Height--;
        }
    }
}
