// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* AccessorRules.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class AccessorRules
{
    #region Private members

    private readonly Cache<Type, Cache<IAccessor, IList<object>>> _rules = new
        (
            _ => new Cache<IAccessor, IList<object>> (_ => new List<object>())
        );

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="rule"></param>
    public void Add
        (
            IAccessor accessor,
            object rule
        )
    {
        Sure.NotNull (accessor);

        _rules[accessor.OwnerType][accessor].Fill (rule);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="rule"></param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>
        (
            Expression<Func<T, object>> expression,
            object rule
        )
    {
        Sure.NotNull (expression);

        Add (expression.ToAccessor(), rule);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRule"></typeparam>
    public void Add<T, TRule>
        (
            Expression<Func<T, object>> expression
        )
        where TRule : new()
    {
        Sure.NotNull (expression);

        Add (expression, new TRule());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T> AllRulesFor<T>
        (
            IAccessor accessor
        )
    {
        Sure.NotNull (accessor);

        return _rules[accessor.OwnerType][accessor].OfType<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T? FirstRule<T>
        (
            IAccessor accessor
        )
    {
        Sure.NotNull (accessor);

        return AllRulesFor<T> (accessor).FirstOrDefault();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRule"></typeparam>
    /// <returns></returns>
    public IEnumerable<TRule> AllRulesFor<T, TRule>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return AllRulesFor<TRule> (expression.ToAccessor());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRule"></typeparam>
    /// <returns></returns>
    public TRule FirstRule<T, TRule>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return FirstRule<TRule> (expression.ToAccessor())!;
    }

    #endregion
}
