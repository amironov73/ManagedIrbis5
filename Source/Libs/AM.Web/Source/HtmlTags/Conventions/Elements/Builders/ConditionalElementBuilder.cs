// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ConditionElementBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

/// <summary>
///
/// </summary>
// Tested through HtmlConventionRegistry
[PublicAPI]
public class ConditionalElementBuilder
    : IElementBuilderPolicy, ITagBuilder
{
    #region Properties

    /// <inheritdoc cref="ConditionDescription"/>
    public string? ConditionDescription { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="inner"></param>
    public ConditionalElementBuilder
        (
            Func<ElementRequest, bool> filter,
            IElementBuilder inner
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (inner);

        _filter = filter;
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly Func<ElementRequest, bool> _filter;

    private readonly IElementBuilder _inner;

    #endregion

    #region ITagBuilder members

    /// <inheritdoc cref="ITagBuilderPolicy.Matches"/>
    public bool Matches
        (
            ElementRequest subject
        )
    {
        Sure.NotNull (subject);

        return _filter (subject);
    }

    /// <inheritdoc cref="ITagBuilderPolicy.BuilderFor"/>
    public ITagBuilder BuilderFor (ElementRequest subject) => this;

    /// <inheritdoc cref="ITagBuilder.Build"/>
    public HtmlTag Build
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        return _inner.Build (request);
    }

    #endregion
}
