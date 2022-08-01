// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* NamedEntityDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Meta info that describes an entity with an explicit string name.
/// Used by tree nodes to enforce entity constraints.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class NamedEntityDefinition<TId, TItem>
    : EntityDefinition<TId, string, TItem>
{
    #region Construction

    /// <summary>
    /// Constructor with explicit Id comparer.
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="getName">Entity name selector</param>
    /// <param name="idComparer">Entity Id comparer</param>
    /// <param name="nameComparer">Entity name comparer (acting as alias comparer)</param>
    public NamedEntityDefinition
        (
            Func<TItem, TId> getId,
            Func<TItem, string> getName,
            IEqualityComparer<TId> idComparer,
            IEqualityComparer<string>? nameComparer = null
        )
        : base (getId, getName, idComparer, nameComparer)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor with default Id comparer.
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="getName">Entity name selector</param>
    /// <param name="nameComparer">Entity name comparer (acting as alias comparer)</param>
    public NamedEntityDefinition
        (
            Func<TItem, TId> getId,
            Func<TItem, string> getName,
            IEqualityComparer<string>? nameComparer = null
        )
        : base (getId, getName, nameComparer)
    {
        // пустое тело конструктора
    }

    #endregion
}
