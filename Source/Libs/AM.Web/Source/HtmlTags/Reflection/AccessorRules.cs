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

namespace AM.HtmlTags.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class AccessorRules
{
    private readonly Cache<Type, Cache<Accessor, IList<object>>> _rules =
        new (
            type => new Cache<Accessor, IList<object>> (a => new List<object>()));

    public void Add (Accessor accessor, object rule)
        => _rules[accessor.OwnerType][accessor].Fill (rule);

    public void Add<T> (Expression<Func<T, object>> expression, object rule)
        => Add (expression.ToAccessor(), rule);

    public void Add<T, TRule> (Expression<Func<T, object>> expression) where TRule : new()
        => Add (expression, new TRule());

    public IEnumerable<T> AllRulesFor<T> (Accessor accessor) => _rules[accessor.OwnerType][accessor].OfType<T>();

    public T FirstRule<T> (Accessor accessor) => AllRulesFor<T> (accessor).FirstOrDefault();

    public IEnumerable<TRule> AllRulesFor<T, TRule> (Expression<Func<T, object>> expression) =>
        AllRulesFor<TRule> (expression.ToAccessor());

    public TRule FirstRule<T, TRule> (Expression<Func<T, object>> expression) =>
        FirstRule<TRule> (expression.ToAccessor());
}
