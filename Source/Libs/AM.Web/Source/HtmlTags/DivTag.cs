// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DivTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags;

/// <summary>
///
/// </summary>
public class DivTag
    : HtmlTag
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public DivTag()
        : base ("div")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="id">Идентификатор элемента.</param>
    public DivTag (string id)
        : base ("div")
    {
        Id (id);
    }

    #endregion
}
