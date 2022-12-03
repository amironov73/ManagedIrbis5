// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* HierarchyPosition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Meta info that describes an entity
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public interface IEntityDefinition<TId, in TItem>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    TId GetId (TItem value);

    /// <summary>
    ///
    /// </summary>
    IEqualityComparer<TId> IdEqualityComparer { get; }

    /// <summary>
    ///
    /// </summary>
    IEqualityComparer<TItem> AliasEqualityComparer { get; }
}

/// <summary>
/// Meta info that describes an entity with an explicit alias (name, title, etc.)
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TItem"></typeparam>
public interface IEntityDefinition<TId, out TName, in TItem>
    : IEntityDefinition<TId, TItem>
{
    /// <summary>
    ///
    /// </summary>
    Func<TItem, TName> GetName { get; }
}
