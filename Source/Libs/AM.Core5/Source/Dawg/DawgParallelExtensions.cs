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

/* DawgParallelExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
public static class DawgParallelExtensions
{
    #region Private methods

    private static TDawgBuilder ToDawgBuilderParallel2<T, TPayload, TDawgBuilder>
        (
            IEnumerable<T> enumerable,
            Func<T, string> key,
            Func<T, TPayload> payload,
            Func<Dictionary<char, Node<TPayload>>, TDawgBuilder> merge
        )
        where TDawgBuilder : DawgBuilder<TPayload>
    {
        var lookup = enumerable.ToLookup (w => key (w).Length > 1);
        var shortKeys = lookup[false];
        var longKeys = lookup[true];

        var dawgBuilders = longKeys
            .GroupBy (item => key (item).First())
            .AsParallel()
            .ToDictionary (g => g.Key, g => g.ToDawgBuilder (item => key (item).Skip (1), payload)._root);

        var dawgBuilder = merge (dawgBuilders);

        foreach (var w in shortKeys)
        {
            dawgBuilder.Insert (key (w), payload (w));
        }

        return dawgBuilder;
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="key"></param>
    /// <param name="payload"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPayload"></typeparam>
    /// <returns></returns>
    public static DawgContainer<TPayload> ToDawgParallel<T, TPayload>
        (
            this IEnumerable<T> enumerable,
            Func<T, string> key,
            Func<T, TPayload> payload
        )
    {
        return ToDawgBuilderParallel (enumerable, key, payload).BuildDawg();
    }

    /// <summary>
    /// Adds all the words in the enumerable to a new DawgBuilder.
    /// </summary>
    /// <remarks>
    /// Splits the word list into groups by the first letter of the word
    /// and calls <see cref="DawgBuilder{TPayload}.Insert"/> for each group in parallel.
    /// </remarks>
    public static DawgBuilder<TPayload> ToDawgBuilderParallel<T, TPayload>
        (
            this IEnumerable<T> enumerable,
            Func<T, string> key,
            Func<T, TPayload> payload
        )
    {
        return ToDawgBuilderParallel2 (enumerable, key, payload, DawgBuilder<TPayload>.Merge);
    }

    /// <summary>
    ///
    /// </summary>
    public static MultiDawgBuilder<TPayload> ToMultiDawgBuilderParallel<T, TPayload>
        (
            this IEnumerable<T> enumerable,
            Func<T, string> key,
            Func<T, IList<TPayload>> payload
        )
    {
        return ToDawgBuilderParallel2 (enumerable, key, payload, MultiDawgBuilder<TPayload>.MergeMulti);
    }

    #endregion
}
