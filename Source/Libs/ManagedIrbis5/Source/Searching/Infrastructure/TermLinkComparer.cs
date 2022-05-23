// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermLinkComparer.cs -- сравнивает термины-ссылки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Сравнивает термины-ссылки по MFN, меткам и повторениям поля
/// и индексу слова.
/// </summary>
public static class TermLinkComparer
{
    #region Nested classes

    /// <summary>
    /// Сравнивает термины-ссылки <see cref="TermLink"/> по MFN.
    /// </summary>
    public sealed class ByMfn
        : IEqualityComparer<TermLink>
    {
        #region IEqualityComparer members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                TermLink? x,
                TermLink? y
            )
        {
            Sure.NotNull (x);
            Sure.NotNull (y);

            return x!.Mfn == y!.Mfn;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                TermLink obj
            )
        {
            Sure.NotNull (obj);

            return obj.Mfn;
        }

        #endregion
    }

    /// <summary>
    /// Сравнивает термины-ссылки <see cref="TermLink"/> по MFN
    /// и метке поля.
    /// </summary>
    public sealed class ByTag
        : IEqualityComparer<TermLink>
    {
        #region IEqualityComparer members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                TermLink? x,
                TermLink? y
            )
        {
            Sure.NotNull (x);
            Sure.NotNull (y);

            return x!.Mfn == y!.Mfn && x.Tag == y.Tag;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                TermLink obj
            )
        {
            Sure.NotNull (obj);

            return unchecked (obj.Mfn * 37 + obj.Tag);
        }

        #endregion
    }

    /// <summary>
    /// Сравнивает термины-ссылки <see cref="TermLink"/> по MFN,
    /// метке поля и его повторению.
    /// </summary>
    public sealed class ByOccurrence
        : IEqualityComparer<TermLink>
    {
        #region IEqualityComparer members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                TermLink? x,
                TermLink? y
            )
        {
            Sure.NotNull (x);
            Sure.NotNull (y);

            return x!.Mfn == y!.Mfn
                   && x.Tag == y.Tag
                   && x.Occurrence == y.Occurrence;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                TermLink obj
            )
        {
            Sure.NotNull (obj);

            return unchecked ((obj.Mfn * 37 + obj.Tag) * 37 + obj.Occurrence);
        }

        #endregion
    }

    /// <summary>
    /// Сравнивает термины-ссылки <see cref="TermLink"/>s по MFN,
    /// метке поля, повторению поля и индексу слова.
    /// </summary>
    public sealed class ByIndex
        : IEqualityComparer<TermLink>
    {
        #region IEqualityComparer members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                TermLink? x,
                TermLink? y
            )
        {
            Sure.NotNull (x);
            Sure.NotNull (y);

            return x!.Mfn == y!.Mfn
                   && x.Tag == y.Tag
                   && x.Occurrence == y.Occurrence
                   && Math.Abs (x.Index - y.Index) == 1;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                TermLink obj
            )
        {
            Sure.NotNull (obj);

            // obj.Index not forgotten!

            return unchecked ((obj.Mfn * 37 + obj.Tag) * 37 + obj.Occurrence);
        }

        #endregion
    }

    #endregion
}
