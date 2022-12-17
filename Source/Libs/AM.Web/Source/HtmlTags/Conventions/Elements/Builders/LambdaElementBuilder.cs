// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LambdaElementBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
//Tested through HtmlConventionRegistry tests
public class LambdaElementBuilder
    : TagBuilder
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? ConditionDescription { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? BuilderDescription { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="build"></param>
    public LambdaElementBuilder
        (
            Func<ElementRequest, HtmlTag> build
        )
        : this (_ => true, build)
    {
        ConditionDescription = "Always";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matcher"></param>
    /// <param name="build"></param>
    public LambdaElementBuilder
        (
            Func<ElementRequest, bool> matcher,
            Func<ElementRequest, HtmlTag> build
        )
    {
        Sure.NotNull (matcher);
        Sure.NotNull (build);

        _matcher = matcher;
        _build = build;
    }

    #endregion

    #region Private members

    private readonly Func<ElementRequest, bool> _matcher;
    private readonly Func<ElementRequest, HtmlTag> _build;

    #endregion

    #region TagBuilder members

    /// <inheritdoc cref="TagBuilder.Matches"/>
    public override bool Matches
        (
            ElementRequest subject
        )
    {
        Sure.NotNull (subject);

        return _matcher (subject);
    }

    /// <inheritdoc cref="TagBuilder.Build"/>
    public override HtmlTag Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        return _build (request);
    }

    #endregion
}
