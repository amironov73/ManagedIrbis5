// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TermComparer.cs -- сравнение терминов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Сравнение терминов.
    /// </summary>
    public static class TermComparer
    {
        #region Nested classes

        /// <summary>
        /// Numeric comparer.
        /// </summary>
        public sealed class Numeric
            : IComparer<BiblioTerm>
        {
            #region IComparer<T> members

            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare (BiblioTerm? x, BiblioTerm? y) =>
                NumberText.Compare (x.ThrowIfNull().Title, y.ThrowIfNull().Title);

            #endregion

        } // class Numeric

        /// <summary>
        /// Trivial comparer.
        /// </summary>
        public sealed class Trivial
            : IComparer<BiblioTerm>
        {
            #region IComparer<T> members

            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare (BiblioTerm? x, BiblioTerm? y) =>
                StringComparer.InvariantCulture.Compare (x.ThrowIfNull().Title, y.ThrowIfNull().Title);

            #endregion

        } // class Trivial

        #endregion

    } // class TermComparer

} // namespace ManagedIrbis.Biblio
