// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ITagBuildingExpression.cs --
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
public interface ITagBuildingExpression
{
    /// <summary>
    ///
    /// </summary>
    CategoryExpression Always { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <returns></returns>
    CategoryExpression If
        (
            Func<ElementRequest, bool> matches
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="builder"></param>
    void Add
        (
            Func<ElementRequest, bool> filter,
            ITagBuilder builder
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="policy"></param>
    void Add
        (
            ITagBuilderPolicy policy
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="modifier"></param>
    void Add
        (
            ITagModifier modifier
        );
}
