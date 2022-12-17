// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CategoryExpression.cs --
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
// Tested through the tests for TagCategory and TagLibrary
public class CategoryExpression
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="matcher"></param>
    public CategoryExpression
        (
            BuilderSet parent,
            Func<ElementRequest, bool> matcher
        )
    {
        Sure.NotNull (parent);
        Sure.NotNull (matcher);

        _parent = parent;
        _matcher = matcher;
    }

    #endregion

    #region Private members

    private readonly BuilderSet _parent;
    private readonly Func<ElementRequest, bool> _matcher;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="modify"></param>
    public void Modify
        (
            Action<ElementRequest> modify
        )
    {
        Sure.NotNull (modify);

        _parent.Add (new LambdaTagModifier (_matcher, modify));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="build"></param>
    public void Build
        (
            Func<ElementRequest, HtmlTag> build
        )
    {
        Sure.NotNull (build);

        _parent.Add (new ConditionalTagBuilderPolicy (_matcher, build));
    }

    #endregion
}
