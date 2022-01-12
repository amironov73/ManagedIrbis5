// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Char and style
/// </summary>
public struct Character
{
    /// <summary>
    /// Unicode character
    /// </summary>
    public char c;

    /// <summary>
    /// Style bit mask
    /// </summary>
    /// <remarks>Bit 1 in position n means that this char will rendering by FastColoredTextBox.Styles[n]</remarks>
    public StyleIndex style;

    public Character (char c)
    {
        this.c = c;
        style = StyleIndex.None;
    }
}
