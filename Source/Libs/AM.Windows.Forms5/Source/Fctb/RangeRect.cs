// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RangeRect.cs -- прямоугольное выделение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Прямоугольное выделение.
/// </summary>
public struct RangeRect
{
    #region Fields

    /// <summary>
    /// Начальная строка.
    /// </summary>
    public int StartLine { get; set; }

    /// <summary>
    /// Начальная колонка.
    /// </summary>
    public int StartChar { get; set; }

    /// <summary>
    /// Конечная строка.
    /// </summary>
    public int EndLine { get; set; }

    /// <summary>
    /// Конечная колонка.
    /// </summary>
    public int EndChar { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="startLine">Начальная строка.</param>
    /// <param name="startChar">Начальная колонка.</param>
    /// <param name="endLine">Конечная строка.</param>
    /// <param name="endChar">Конечная колонка.</param>
    public RangeRect
        (
            int startLine,
            int startChar,
            int endLine,
            int endChar
        )
    {
        StartLine = startLine;
        StartChar = startChar;
        EndLine = endLine;
        EndChar = endChar;
    }

    #endregion
}
