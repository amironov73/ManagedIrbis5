// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable UnusedMember.Global

/* IdentityError.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
///
/// </summary>
[Flags]
public enum IdentityError
{
    /// <summary>
    ///
    /// </summary>
    None = 0,

    /// <summary>
    ///
    /// </summary>
    SiblingIdDuplicate = 0x1,

    /// <summary>
    ///
    /// </summary>
    SiblingAliasDuplicate = 0x2,

    /// <summary>
    ///
    /// </summary>
    CyclicIdDuplicate = 0x4,

    /// <summary>
    ///
    /// </summary>
    TreeScopeIdDuplicate = 0x8
}

/// <summary>
///
/// </summary>
public static class IdentityErrorExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IdentityError Normalize
        (
            this IdentityError source
        )
    {
        var filteredIdVersion = source & ~IdentityError.TreeScopeIdDuplicate & ~IdentityError.SiblingAliasDuplicate;

        return filteredIdVersion == IdentityError.None ? source : filteredIdVersion;
    }
}
