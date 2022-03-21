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

/* ButtonArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
///
/// </summary>
public sealed class ButtonArgs
{
    public bool CloseAfterClick { get; set; }

    public IButton Button { get; }

    public ButtonArgs(IButton button)
    {
        Button = button;
        CloseAfterClick = true;
    }
}
