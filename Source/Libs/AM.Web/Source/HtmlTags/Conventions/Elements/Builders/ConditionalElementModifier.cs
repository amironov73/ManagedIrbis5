// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ConditionalElementModifier.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements.Builders;

#region Using directives

using System;

#endregion

/// <summary>
///
/// </summary>
// Tested through HtmlConventionRegistry
[PublicAPI]
public class ConditionalElementModifier
    : IElementModifier
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? ConditionDescription { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="inner"></param>
    public ConditionalElementModifier
        (
            Func<ElementRequest, bool> filter,
            IElementModifier inner
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (inner);

        _filter = filter;
        _inner = inner;
    }

    #endregion

    private readonly Func<ElementRequest, bool> _filter;

    private readonly IElementModifier _inner;

    #region ITagBuilder members

    /// <inheritdoc cref="ITagModifier.Matches"/>
    public bool Matches
        (
            ElementRequest token
        )
    {
        Sure.NotNull (token);

        return _filter (token);
    }

    /// <inheritdoc cref="ITagModifier.Modify"/>
    public void Modify
        (
            ElementRequest request
        )
    {
        Sure.NotNull (request);

        _inner.Modify (request);
    }

    #endregion
}
