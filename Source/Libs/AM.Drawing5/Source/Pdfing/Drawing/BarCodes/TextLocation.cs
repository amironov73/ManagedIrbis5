// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TextLocation.cs -- определяет, будет ли отображаться текст в штрих-коде и каким образом.
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Определяет, будет ли отображаться текст в штрих-коде и каким образом.
/// </summary>
public enum TextLocation
{
    /// <summary>
    /// Текст не будет отображаться.
    /// </summary>
    None,

    /// <summary>
    /// Текст над штрих-кодом.
    /// </summary>
    Above,

    /// <summary>
    /// Текст под штрих-кодом.
    /// </summary>
    Below,

    /// <summary>
    /// Текст над штрих-кодом, внутри него.
    /// </summary>
    AboveEmbedded,


    /// <summary>
    /// Текст под штрих-кодом, внутри него.
    /// </summary>
    BelowEmbedded
}
