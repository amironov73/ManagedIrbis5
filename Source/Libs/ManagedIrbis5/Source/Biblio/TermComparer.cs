// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermComparer.cs -- сравнение терминов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Сравнение терминов.
/// </summary>
[PublicAPI]
public static class TermComparer
{
    #region Nested classes

    /// <summary>
    /// Сравнение терминов как чисел.
    /// </summary>
    public sealed class Numeric
        : IComparer<BiblioTerm>
    {
        #region IComparer<T> members

        /// <inheritdoc cref="IComparer{T}.Compare" />
        [Pure]
        public int Compare
            (
                BiblioTerm? x,
                BiblioTerm? y
            )
        {
            return NumberText.Compare
                (
                    x?.Title,
                    y?.Title
                );
        }

        #endregion
    }

    /// <summary>
    /// Тривиальное сравнение терминов.
    /// </summary>
    public sealed class Trivial
        : IComparer<BiblioTerm>
    {
        #region IComparer<T> members

        /// <inheritdoc cref="IComparer{T}.Compare" />
        [Pure]
        public int Compare
            (
                BiblioTerm? x,
                BiblioTerm? y
            )
        {
            return StringComparer.InvariantCulture.Compare
                (
                    x?.Title,
                    y?.Title
                );
        }

        #endregion
    }

    #endregion
}
