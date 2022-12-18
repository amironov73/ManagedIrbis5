// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LambdaTagBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class LambdaTagBuilder
    : ITagBuilder
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="build"></param>
    public LambdaTagBuilder
        (
            Func<ElementRequest, HtmlTag> build
        )
    {
        Sure.NotNull (build);

        _build = build;
    }

    #endregion

    #region Private members

    private readonly Func<ElementRequest, HtmlTag> _build;

    #endregion

    #region ITagBuilder methods

    /// <inheritdoc cref="ITagBuilder.Build"/>
    public HtmlTag Build (ElementRequest request) => _build (request);

    #endregion
}
