// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BlockType.cs -- типы текстовых блоков
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.Layout;

/// <summary>
/// Типы текстовых блоков.
/// </summary>
internal enum BlockType
{
    /// <summary>
    /// Собственно текст.
    /// </summary>
    Text,

    /// <summary>
    /// Пробел.
    /// </summary>
    Space,

    /// <summary>
    /// Дефис.
    /// </summary>
    Hyphen,

    /// <summary>
    /// Разрыв строки.
    /// </summary>
    LineBreak
}
