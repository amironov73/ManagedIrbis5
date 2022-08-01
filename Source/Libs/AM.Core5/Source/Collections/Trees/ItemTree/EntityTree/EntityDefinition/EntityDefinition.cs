// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* EntityDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Meta info that describes an entity.
/// Used by tree nodes to enforce entity constraints.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class EntityDefinition<TId, TItem>
    : IEntityDefinition<TId, TItem>
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public IEqualityComparer<TId> IdEqualityComparer { get; }

    /// <summary>
    ///
    /// </summary>
    public IEqualityComparer<TItem> AliasEqualityComparer { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor with explicit Id Comparer
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="idComparer">Entity Id comparer</param>
    /// <param name="aliasComparer">Entity alias comparer (without explicit name)</param>
    public EntityDefinition
        (
            Func<TItem, TId> getId,
            IEqualityComparer<TId> idComparer,
            IEqualityComparer<TItem>? aliasComparer = null
        )
    {
        Sure.NotNull (getId);
        Sure.NotNull (idComparer);

        _getId = getId;
        IdEqualityComparer = idComparer;
        AliasEqualityComparer = aliasComparer ?? EqualityComparer<TItem>.Default;
    }

    /// <summary>
    /// Constructor with default Id Comparer
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="aliasComparer">Entity alias comparer (without explicit name)</param>
    public EntityDefinition
        (
            Func<TItem, TId> getId,
            IEqualityComparer<TItem>? aliasComparer = null
        )
        : this (getId, EqualityComparer<TId>.Default, aliasComparer)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private readonly Func<TItem, TId> _getId;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public TId GetId (TItem value) => _getId (value);

    #endregion
}

/// <summary>
/// Meta info that describes an entity with an explicit name of arbitrary type.
/// Used by tree nodes to enforce entity constraints.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class EntityDefinition<TId, TName, TItem>
    : EntityDefinition<TId, TItem>,
    IEntityDefinition<TId, TName, TItem>
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public Func<TItem, TName> GetName { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor with explicit Id comparer
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="getName">Entity name selector</param>
    /// <param name="idComparer">Entity id comparer</param>
    /// <param name="nameComparer">Name comparer (acting as alias comparer)</param>
    public EntityDefinition
        (
            Func<TItem, TId> getId,
            Func<TItem, TName> getName,
            IEqualityComparer<TId>? idComparer,
            IEqualityComparer<TName>? nameComparer = null
        )
        : base
            (
                getId,
                idComparer ?? EqualityComparer<TId>.Default,
                nameComparer != null ? new AliasComparer<TItem, TName> (getName, nameComparer) : null
            )
    {
        GetName = getName;
    }

    /// <summary>
    /// Constructor with default Id comparer.
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="getName">Entity name selector</param>
    /// <param name="nameComparer">Name comparer (acting as alias comparer)</param>
    public EntityDefinition
        (
            Func<TItem, TId> getId,
            Func<TItem, TName> getName,
            IEqualityComparer<TName>? nameComparer = null
        )
        : this (getId, getName, null, nameComparer)
    {
        // пустое тело конструктора
    }

    #endregion
}
