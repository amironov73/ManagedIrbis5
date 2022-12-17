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

/* BuilderDef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

using Elements;

/// <summary>
///
/// </summary>
// Tested through the test for TagCategory and TagLibrary
public class BuilderSet
    : ITagBuildingExpression
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<ITagBuilderPolicy> Policies => _policies;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<ITagModifier> Modifiers => _modifiers;

    /// <summary>
    ///
    /// </summary>
    public CategoryExpression Always => new (this, _ => true);

    /// <summary>
    ///
    /// </summary>
    public IElementNamingConvention ElementNamingConvention => _elementNamingConvention;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public BuilderSet()
    {
        _elementNamingConvention = new DefaultElementNamingConvention();
    }

    #endregion

    #region Private members

    private readonly List<ITagBuilderPolicy> _policies = new ();

    private readonly List<ITagModifier> _modifiers = new ();

    private IElementNamingConvention _elementNamingConvention;

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
            ITagBuilder builder
        )
    {
        Sure.NotNull (filter);
        Sure.NotNull (builder);

        _policies.Add (new ConditionalTagBuilderPolicy (filter, builder));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="policy"></param>
    public void Add
        (
            ITagBuilderPolicy policy
        )
    {
        Sure.NotNull (policy);

        _policies.Add (policy);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modifier"></param>
    public void Add
        (
            ITagModifier modifier
        )
    {
        Sure.NotNull (modifier);

        _modifiers.Add (modifier);
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

        _elementNamingConvention = elementNamingConvention;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="matches"></param>
    /// <returns></returns>
    public CategoryExpression If
        (
            Func<ElementRequest, bool> matches
        )
    {
        Sure.NotNull (matches);

        return new (this, matches);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    public void Import
        (
            BuilderSet other
        )
    {
        Sure.NotNull (other);

        _policies.AddRange (other._policies);
        _modifiers.AddRange (other._modifiers);
        _elementNamingConvention = other._elementNamingConvention;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="policy"></param>
    public void InsertFirst
        (
            ITagBuilderPolicy policy
        )
    {
        Sure.NotNull (policy);

        _policies.Insert (0, policy);
    }

    #endregion
}
