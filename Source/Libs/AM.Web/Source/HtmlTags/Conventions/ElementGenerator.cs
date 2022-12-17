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

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

using System;
using System.Linq.Expressions;

using Elements;

using Reflection;

public class ElementGenerator<T> : IElementGenerator<T> where T : class
{
    private readonly ITagGenerator _tags;
    private Lazy<T> _model;

    private ElementGenerator (ITagGenerator tags)
    {
        _tags = tags;
    }

    public static ElementGenerator<T> For (HtmlConventionLibrary library, Func<Type, object>? serviceLocator = null,
        T? model = null)
    {
        serviceLocator = serviceLocator ?? Activator.CreateInstance;

        var tags = new TagGenerator (library.TagLibrary, new ActiveProfile(), serviceLocator);

        return new ElementGenerator<T> (tags)
        {
            Model = model
        };
    }

    public HtmlTag LabelFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Label, profile, model);

    public HtmlTag InputFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Editor, profile, model);

    public HtmlTag ValidationMessageFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.ValidationMessage, profile, model);

    public HtmlTag DisplayFor<TResult> (Expression<Func<T, TResult>> expression, string? profile = null,
        T? model = null)
        => Build (expression, ElementConstants.Display, profile, model);

    public HtmlTag TagFor<TResult> (Expression<Func<T, TResult>> expression, string category, string? profile = null,
        T? model = null)
        => Build (expression, category, profile, model);

    public T? Model
    {
        get => _model.Value;
        set => _model = new Lazy<T> (() => value);
    }

    public ElementRequest GetRequest<TResult> (Expression<Func<T, TResult>> expression, T? model = null)
    {
        return new (expression.ToAccessor())
        {
            Model = model ?? Model
        };
    }

    private HtmlTag Build<TResult> (Expression<Func<T, TResult>> expression, string category, string? profile = null,
        T? model = null)
    {
        ElementRequest request = GetRequest (expression, model);
        return _tags.Build (request, category, profile);
    }

    private HtmlTag Build (ElementRequest request, string category, string? profile = null, T? model = null)
    {
        request.Model = model ?? Model;
        return _tags.Build (request, category, profile: profile);
    }

    // Below methods are tested through the IFubuPage.Show/Edit method tests
    public HtmlTag LabelFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Label, profile, model);

    public HtmlTag InputFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Editor, profile, model);

    public HtmlTag ValidationMessageFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.ValidationMessage, profile, model);

    public HtmlTag DisplayFor (ElementRequest request, string? profile = null, T? model = null) =>
        Build (request, ElementConstants.Display, profile, model);

    public HtmlTag TagFor (ElementRequest request, string category, string? profile = null, T? model = null) =>
        Build (request, category, profile, model);
}
