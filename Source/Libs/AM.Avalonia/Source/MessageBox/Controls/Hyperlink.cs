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

/* Hyperlink.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

using AM.Avalonia.Extensions;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
///
/// </summary>
public class Hyperlink
    : TextBlock
{
    public static readonly DirectProperty<Hyperlink, string> UrlProperty
        = AvaloniaProperty.RegisterDirect<Hyperlink, string> (nameof (Url), o => o.Url, (o, v) => o.Url = v);

    private string _url;

    public string Url
    {
        get => _url;
        set => SetAndRaise (UrlProperty, ref _url, value);
    }

    /// <inheritdoc cref="InputElement.OnPointerPressed"/>
    protected override void OnPointerPressed
        (
            PointerPressedEventArgs e
        )
    {
        base.OnPointerPressed (e);
        if (!string.IsNullOrEmpty (Url))
        {
            Url.OpenUrl();
        }
    }
}
