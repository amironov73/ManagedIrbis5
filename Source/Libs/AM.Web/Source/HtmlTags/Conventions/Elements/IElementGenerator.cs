// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IElementGenerator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IElementGenerator<T>
    where T : class
{
    /// <summary>
    ///
    /// </summary>
    T Model { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    HtmlTag LabelFor<TResult>
        (
            Expression<Func<T, TResult>> expression,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    HtmlTag InputFor<TResult>
        (
            Expression<Func<T, TResult>> expression,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    HtmlTag ValidationMessageFor<TResult>
        (
            Expression<Func<T, TResult>> expression,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    HtmlTag DisplayFor<TResult>
        (
            Expression<Func<T, TResult>> expression,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    HtmlTag TagFor<TResult>
        (
            Expression<Func<T, TResult>> expression,
            string category,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    HtmlTag LabelFor
        (
            ElementRequest request,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    HtmlTag InputFor
        (
            ElementRequest request,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    HtmlTag ValidationMessageFor
        (
            ElementRequest request,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    HtmlTag DisplayFor
        (
            ElementRequest request,
            string? profile = null,
            T? model = null
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    HtmlTag TagFor
        (
            ElementRequest request,
            string category,
            string? profile = null,
            T? model = null
        );
}
