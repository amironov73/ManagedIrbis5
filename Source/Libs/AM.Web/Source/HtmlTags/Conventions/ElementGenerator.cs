// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ElementGenerator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

#region Using directives

using Elements;
using Reflection;

#endregion

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ElementGenerator<T>
    : IElementGenerator<T> where T : class
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public T Model
    {
        get => _model!.Value;
        set => _model = new Lazy<T> (() => value);
    }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="tags"></param>
    private ElementGenerator
        (
            ITagGenerator tags
        )
    {
        _tags = tags;
    }

    #endregion

    #region Private members

    private readonly ITagGenerator _tags;
    private Lazy<T>? _model;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    /// <param name="serviceLocator"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static ElementGenerator<T> For
        (
            HtmlConventionLibrary library, Func<Type, object>? serviceLocator = null,
            T? model = null
        )
    {
        serviceLocator = serviceLocator ?? Activator.CreateInstance;

        var tags = new TagGenerator (library.TagLibrary, new ActiveProfile(), serviceLocator);

        return new ElementGenerator<T> (tags)
        {
            Model = model
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public HtmlTag LabelFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Label, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public HtmlTag InputFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Editor, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public HtmlTag ValidationMessageFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.ValidationMessage, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public HtmlTag DisplayFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Display, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public HtmlTag TagFor<TResult> (Expression<Func<T, TResult>> expression, string category, string? profile = null,
        T? model = null)
        => Build (expression, category, profile, model);


    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public ElementRequest GetRequest<TResult> (Expression<Func<T, TResult>> expression, T? model = null)
    {
        return new (expression.ToAccessor())
        {
            Model = model ?? Model
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    private HtmlTag Build<TResult> (Expression<Func<T, TResult>> expression, string category, string? profile = null,
        T? model = null)
    {
        var request = GetRequest (expression, model);
        return _tags.Build (request, category, profile);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private HtmlTag Build (ElementRequest request, string category, string? profile = null, T? model = null)
    {
        request.Model = model ?? Model;
        return _tags.Build (request, category, profile: profile);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    // Below methods are tested through the IFubuPage.Show/Edit method tests
    public HtmlTag LabelFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Label, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public HtmlTag InputFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Editor, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public HtmlTag ValidationMessageFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.ValidationMessage, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public HtmlTag DisplayFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Display, profile, model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="category"></param>
    /// <param name="profile"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public HtmlTag TagFor (ElementRequest request, string category, string? profile = null, T? model = null) =>
        Build (request, category, profile, model);
}
