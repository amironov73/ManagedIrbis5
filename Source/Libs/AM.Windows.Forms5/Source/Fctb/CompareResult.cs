// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CompareResult.cs -- результат сравнения
 * Ars Magna project, http://arsmagna.ru
 */

namespace Fctb;

/// <summary>
/// Результат сравнения.
/// </summary>
public enum CompareResult
{
    /// <summary>
    /// Item do not appears
    /// </summary>
    Hidden,

    /// <summary>
    /// Item appears
    /// </summary>
    Visible,

    /// <summary>
    /// Item appears and will selected
    /// </summary>
    VisibleAndSelected
}
