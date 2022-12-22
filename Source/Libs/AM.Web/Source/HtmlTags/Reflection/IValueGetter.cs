// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IValueGetter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public interface IValueGetter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    object? GetValue (object target);

    /// <summary>
    ///
    /// </summary>
    string Name { get; }

    /// <summary>
    ///
    /// </summary>
    Type? DeclaringType { get; }

    /// <summary>
    ///
    /// </summary>
    Type? ValueType { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="body"></param>
    /// <returns></returns>
    Expression ChainExpression (Expression body);

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="propertyValue"></param>
    void SetValue (object target, object? propertyValue);
}
