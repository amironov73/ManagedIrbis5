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

/* DefaultButton.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Avalonia.Buttons;

/// <summary>
///
/// </summary>
public static class DefaultButtons
{
    /// <summary>
    ///
    /// </summary>
    public static Button OkButton => new OkButtonImpl();

    /// <summary>
    ///
    /// </summary>
    public static Button CancelButton => new CancelButtonImpl();

    /// <summary>
    ///
    /// </summary>
    private class OkButtonImpl
        : Button
    {
        public OkButtonImpl()
        {
            Name = "Ok";
            IsDefault = true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    private class CancelButtonImpl
        : Button
    {
        public CancelButtonImpl()
        {
            Name = "Cancel";
            IsCancel = true;
        }
    }
}
