// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BrTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags;

/// <summary>
///
/// </summary>
public class BrTag
    : HtmlTag
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BrTag()
        : base ("br")
    {
        // пустое тело конструктора
    }

    #endregion
}
