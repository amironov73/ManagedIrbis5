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

namespace AM.HtmlTags.Conventions.Elements;

using System;
using System.Linq.Expressions;

public interface IElementGenerator<T> where T : class
{
    T Model { get; set; }
    HtmlTag LabelFor<TResult> (Expression<Func<T, TResult>> expression, string profile = null, T model = null);
    HtmlTag InputFor<TResult> (Expression<Func<T, TResult>> expression, string profile = null, T model = null);

    HtmlTag ValidationMessageFor<TResult> (Expression<Func<T, TResult>> expression, string profile = null,
        T model = null);

    HtmlTag DisplayFor<TResult> (Expression<Func<T, TResult>> expression, string profile = null, T model = null);

    HtmlTag TagFor<TResult> (Expression<Func<T, TResult>> expression, string category, string profile = null,
        T model = null);

    HtmlTag LabelFor (ElementRequest request, string profile = null, T model = null);
    HtmlTag InputFor (ElementRequest request, string profile = null, T model = null);
    HtmlTag ValidationMessageFor (ElementRequest request, string profile = null, T model = null);
    HtmlTag DisplayFor (ElementRequest request, string profile = null, T model = null);
    HtmlTag TagFor (ElementRequest request, string category, string profile = null, T model = null);
}
