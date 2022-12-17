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

/* ElementCategoryExpression.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

using System;

using Elements;

using Reflection;

/// <summary>
///
/// </summary>
public class ElementCategoryExpression
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public ElementActionExpression Always
    {
        get { return new (_set, _ => true, "Always"); }
    }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="set"></param>
    public ElementCategoryExpression
        (
            BuilderSet set
        )
    {
        Sure.NotNull (set);

        _set = set;
    }

    #endregion

    #region Private members

    private readonly BuilderSet _set;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="builder"></param>
    public void Add
        (
            Func<ElementRequest, bool> filter,
            IElementBuilder builder
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (builder);

        _set.Add (filter, builder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="policy"></param>
    public void Add
        (
            IElementBuilderPolicy policy
        )
    {
        Sure.NotNull (policy);

        _set.Add (policy);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modifier"></param>
    public void Add
        (
            IElementModifier modifier
        )
    {
        Sure.NotNull (modifier);

        _set.Add (modifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void BuilderPolicy<T>()
        where T : IElementBuilderPolicy, new()
    {
        Add (new T());
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Modifier<T>()
        where T : IElementModifier, new()
    {
        Add (new T());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="elementNamingConvention"></param>
    public void NamingConvention
        (
            IElementNamingConvention elementNamingConvention
        )
    {
        Sure.NotNull (elementNamingConvention);

        _set.NamingConvention (elementNamingConvention);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public ElementActionExpression If
        (
            Func<ElementRequest, bool> matches,
            string? description = null
        )
    {
        Sure.NotNull (matches);

        return new (_set, matches, description);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ElementActionExpression IfPropertyIs<T>()
    {
        return If (req => req.Accessor.PropertyType == typeof (T),
            $"Property type is {typeof (T).Name}");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public ElementActionExpression IfPropertyTypeIs (Func<Type, bool> matches, string? description = null)
    {
        return If (def => matches (def.Accessor.PropertyType), description);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ElementActionExpression IfPropertyHasAttribute<T>() where T : Attribute
    {
        return If (req => req.Accessor.HasAttribute<T>(), $"Accessor has attribute [{typeof (T).Name}]");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    /// <typeparam name="T"></typeparam>
    public void AddClassForAttribute<T> (string className) where T : Attribute
    {
        IfPropertyHasAttribute<T>().AddClass (className);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modification"></param>
    /// <param name="description"></param>
    /// <typeparam name="T"></typeparam>
    public void ModifyForAttribute<T> (Action<HtmlTag, T> modification, string? description = null)
        where T : Attribute
    {
        IfPropertyHasAttribute<T>().ModifyWith (req =>
        {
            var att = req.Accessor.GetAttribute<T>();
            modification (req.CurrentTag, att);
        }, description);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modification"></param>
    /// <param name="description"></param>
    /// <typeparam name="T"></typeparam>
    public void ModifyForAttribute<T> (Action<HtmlTag> modification, string? description = null) where T : Attribute
    {
        ModifyForAttribute<T> ((tag, att) => modification (tag), description);
    }

    #endregion
}
