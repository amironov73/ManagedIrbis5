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

/* DawgExtensions.cs --
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
public static class DawgExtensions
{
    public static DawgContainer<TPayload> ToDawg<T, TPayload> (this IEnumerable<T> enumerable, Func<T, IEnumerable<char>> key,
        Func<T, TPayload> payload)
    {
        var dawgBuilder = ToDawgBuilder (enumerable, key, payload);

        return dawgBuilder.BuildDawg();
    }

    public static DawgBuilder<TPayload> ToDawgBuilder<T, TPayload> (this IEnumerable<T> enumerable,
        Func<T, IEnumerable<char>> key, Func<T, TPayload> payload)
    {
        var dawgBuilder = new DawgBuilder<TPayload>();

        foreach (var elem in enumerable)
        {
            dawgBuilder.Insert (key (elem), payload (elem));
        }

        return dawgBuilder;
    }
}
