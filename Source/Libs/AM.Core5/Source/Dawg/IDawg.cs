// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* IDawg.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal interface IDawg<TPayload>
{
    TPayload this [IEnumerable<char> word] { get; }

    int GetLongestCommonPrefixLength (IEnumerable<char> word);

    /// <summary>
    /// Returns all elements with key matching given <paramref name="prefix"/>.
    /// </summary>
    IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix (IEnumerable<char> prefix);

    IEnumerable<KeyValuePair<string, TPayload>> GetPrefixes (IEnumerable<char> key);

    int GetNodeCount();

    KeyValuePair<string, TPayload> GetRandomItem (Random random);
}
