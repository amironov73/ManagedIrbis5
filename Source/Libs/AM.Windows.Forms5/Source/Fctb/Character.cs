// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Character.cs -- символ и его стиль
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Символ и его стиль.
/// </summary>
public struct Character
{
    #region Properties

    /// <summary>
    /// Unicode-символ.
    /// </summary>
    public char c;

    /// <summary>
    /// Битовая маска стилей.
    /// </summary>
    /// <remarks>Bit 1 in position n means that this char will rendering by FastColoredTextBox.Styles[n]</remarks>
    public StyleIndex style;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Character
        (
            char c
        )
    {
        this.c = c;
        style = StyleIndex.None;
    }

    #endregion
}
