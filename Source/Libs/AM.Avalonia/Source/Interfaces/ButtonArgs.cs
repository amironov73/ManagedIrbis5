// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
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
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public bool CloseAfterClick { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IButton Button { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="button">Кнопка.</param>
    public ButtonArgs
        (
            IButton button
        )
    {
        Sure.NotNull (button);

        Button = button;
        CloseAfterClick = true;
    }

    #endregion
}
