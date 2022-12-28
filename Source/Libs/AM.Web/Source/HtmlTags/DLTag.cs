// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DLTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags;

/// <summary>
///
/// </summary>
public class DLTag
    : HtmlTag
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public DLTag()
        : base ("dl")
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configure"></param>
    public DLTag
        (
            Action<DLTag> configure
        )
        : this()
    {
        Sure.NotNull (configure);

        configure (this);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="term"></param>
    /// <param name="definition"></param>
    /// <returns></returns>
    public DLTag AddDefinition
        (
            string term,
            string definition
        )
    {
        Sure.NotNullNorEmpty (term);
        Sure.NotNullNorEmpty (definition);

        Add ("dt").Text (term);
        Add ("dd").Text (definition);

        return this;
    }

    #endregion
}
