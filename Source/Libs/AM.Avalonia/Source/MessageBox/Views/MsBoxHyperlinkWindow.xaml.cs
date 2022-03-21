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

/* MsBoxHyperlinkWindow.xaml.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using AM.Avalonia.BaseWindows.Base;
using AM.Avalonia.Controls;
using AM.Avalonia.Enums;

namespace AM.Avalonia.Views;

#endregion

/// <summary>
///
/// </summary>
public class MsBoxHyperlinkWindow
    : BaseWindow, IWindowGetResult<ButtonResult>
{
    /// <summary>
    ///
    /// </summary>
    public MsBoxHyperlinkWindow()
    {
        this.InitializeComponent();
    }

    public ButtonResult ButtonResult { get; set; } = ButtonResult.None;

    public ButtonResult GetResult() => ButtonResult;


    //More like a workaround because i dont know how to set it only with styles in .xaml file
    protected override void OnOpened (EventArgs e)
    {
        var res = this.Find<ItemsControl> ("myItems");
        var temp = res?.GetLogicalChildren();
        if (temp != null)
        {
            foreach (var logical in temp)
            {
                var item = (ContentPresenter)logical;
                if (item.Content is Models.HyperlinkContent content && item.Child is Hyperlink hyperlink)
                {
                    var isAliasEmpty = string.IsNullOrEmpty (content.Alias);
                    var isUrlEmpty = string.IsNullOrEmpty (content.Url);
                    if (isAliasEmpty && !isUrlEmpty)
                    {
                        hyperlink.Text = content.Url;
                    }
                    else if (!isAliasEmpty && isUrlEmpty)
                    {
                        hyperlink.Foreground = new SolidColorBrush (Color.Parse ("Black"));
                        hyperlink.TextDecorations = new TextDecorationCollection();
                        hyperlink.Cursor = new Cursor (StandardCursorType.Arrow);
                    }
                }
            }
        }

        base.OnOpened (e);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load (this);
    }
}
