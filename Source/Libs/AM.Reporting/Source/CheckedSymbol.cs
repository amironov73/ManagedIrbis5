// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies a symbol that will be displayed when a <see cref="CheckBoxObject"/>
/// is in the checked state.
/// </summary>
public enum CheckedSymbol
{
    /// <summary>
    /// Specifies a check symbol.
    /// </summary>
    Check,

    /// <summary>
    /// Specifies a diagonal cross symbol.
    /// </summary>
    Cross,

    /// <summary>
    /// Specifies a plus symbol.
    /// </summary>
    Plus,

    /// <summary>
    /// Specifies a filled rectangle.
    /// </summary>
    Fill
}
