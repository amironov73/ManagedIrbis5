// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ITagBuilder.cs --
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
public interface ITagBuilderPolicy
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    bool Matches (ElementRequest subject);

    /// <summary>
    ///
    /// </summary>
    /// <param name="subject"></param>
    /// <returns></returns>
    ITagBuilder BuilderFor (ElementRequest subject);
}

/// <summary>
///
/// </summary>
public class ConditionalTagBuilderPolicy
    : ITagBuilderPolicy
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="builder"></param>
    public ConditionalTagBuilderPolicy
        (
            Func<ElementRequest, bool> filter,
            Func<ElementRequest, HtmlTag> builder
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (builder);

        _filter = filter;
        _builder = new LambdaTagBuilder (builder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="builder"></param>
    public ConditionalTagBuilderPolicy
        (
            Func<ElementRequest, bool> filter,
            ITagBuilder builder
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (builder);

        _filter = filter;
        _builder = builder;
    }

    #endregion

    #region Private members

    private readonly Func<ElementRequest, bool> _filter;
    private readonly ITagBuilder _builder;

    #endregion

    #region ITagBuilderPolicy members

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
    public ITagBuilder BuilderFor (ElementRequest subject) => _builder;

    #endregion
}

/// <summary>
///
/// </summary>
public interface ITagBuilder
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    HtmlTag Build (ElementRequest request);
}

/// <summary>
///
/// </summary>
public abstract class TagBuilder
    : ITagBuilderPolicy, ITagBuilder
{
    #region ITagBuilderPolicy members

    /// <inheritdoc cref="ITagBuilderPolicy.Matches"/>
    public abstract bool Matches (ElementRequest subject);

    /// <inheritdoc cref="ITagBuilderPolicy.BuilderFor"/>
    public ITagBuilder BuilderFor (ElementRequest subject) => this;

    #endregion

    #region ITagBuilder

    /// <inheritdoc cref="ITagBuilder.Build"/>
    public abstract HtmlTag Build (ElementRequest request);

    #endregion
}
