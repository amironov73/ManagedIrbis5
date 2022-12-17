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

using Elements;
using Elements.Builders;

/// <summary>
///
/// </summary>
public class HtmlConventionRegistry
    : ProfileExpression
{
    /// <summary>
    ///
    /// </summary>
    public HtmlConventionRegistry()
        : this (new HtmlConventionLibrary())
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="library"></param>
    private HtmlConventionRegistry
        (
            HtmlConventionLibrary library
        )
        : base (library, TagConstants.Default)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="profileName"></param>
    /// <param name="configure"></param>
    public void Profile
        (
            string profileName,
            Action<ProfileExpression> configure
        )
    {
        Sure.NotNullNorEmpty (profileName);
        Sure.NotNull (configure);

        var expression = new ProfileExpression (Library, profileName);
        configure (expression);
    }
}

/// <summary>
///
/// </summary>
public class ElementActionExpression
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="set"></param>
    /// <param name="filter"></param>
    /// <param name="filterDescription"></param>
    public ElementActionExpression
        (
            BuilderSet set,
            Func<ElementRequest, bool> filter,
            string? filterDescription
        )
    {
        Sure.NotNull (set);
        Sure.NotNull (filter);

        _set = set;
        _filter = filter;
        _filterDescription = Utility.NonEmpty (filterDescription, "User defined");
    }

    #endregion

    #region Private members

    private readonly BuilderSet _set;
    private readonly Func<ElementRequest, bool> _filter;
    private readonly string _filterDescription;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    public void BuildBy
        (
            IElementBuilder builder
        )
    {
        Sure.NotNull (builder);

        var conditionalBuilder = new ConditionalElementBuilder (_filter, builder)
        {
            ConditionDescription = _filterDescription
        };

        _set.Add (conditionalBuilder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void BuildBy<T>()
        where T : IElementBuilder, new()
    {
        BuildBy (new T());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tagBuilder"></param>
    /// <param name="description"></param>
    public void BuildBy
        (
            Func<ElementRequest, HtmlTag> tagBuilder,
            string? description = null
        )
    {
        Sure.NotNull (tagBuilder);

        var builder = new LambdaElementBuilder (_filter, tagBuilder)
        {
            ConditionDescription = _filterDescription,
            BuilderDescription = description ?? "User Defined"
        };

        _set.Add (builder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modifier"></param>
    public void ModifyWith
        (
            IElementModifier modifier
        )
    {
        Sure.NotNull (modifier);

        var conditionalModifier = new ConditionalElementModifier (_filter, modifier)
        {
            ConditionDescription = _filterDescription
        };

        _set.Add (conditionalModifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ModifyWith<T>()
        where T : IElementModifier, new()
    {
        ModifyWith (new T());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modification"></param>
    /// <param name="description"></param>
    public void ModifyWith
        (
            Action<ElementRequest> modification,
            string? description = null
        )
    {
        Sure.NotNull (modification);

        var modifier = new LambdaElementModifier (_filter, modification)
        {
            ConditionDescription = _filterDescription,
            ModifierDescription = description ?? "User defined"
        };

        _set.Add (modifier);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="attributeName"></param>
    /// <param name="value"></param>
    public void Attr
        (
            string attributeName,
            object value
        )
    {
        Sure.NotNullNorEmpty (attributeName);

        ModifyWith
            (
                req => req.CurrentTag.Attr (attributeName, value),
                string.Format ("Set @{0} = '{1}'", new[] { attributeName, value })
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="className"></param>
    public void AddClass
        (
            string className
        )
    {
        Sure.NotNullNorEmpty (className);

        ModifyWith
            (
                req => req.CurrentTag.AddClass (className),
                string.Format ("Add class '{0}'", new[] { className })
            );
    }

    #endregion
}
