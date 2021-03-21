// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* RecordComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    public static class RecordComparer
    {
        #region Nested classes

        sealed class ByMfnComparer
            : IComparer<Record>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    Record? x,
                    Record? y
                )
            {
                return (x?.Mfn ?? 0) - (y?.Mfn ?? 0);
            }
        }

        sealed class ByIndexComparer
            : IComparer<Record>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    Record? x,
                    Record? y
                )
            {
                return string.CompareOrdinal
                    (
                        x?.Index,
                        y?.Index
                    );
            }
        }

        sealed class ByDescriptionComparer
            : IComparer<Record>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    Record? x,
                    Record? y
                )
            {
                return string.CompareOrdinal
                    (
                        x?.Description,
                        y?.Description
                    );
            }
        }

        sealed class BySortKeyComparer
            : IComparer<Record>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    Record? x,
                    Record? y
                )
            {
                return string.CompareOrdinal
                    (
                        x?.SortKey,
                        y?.SortKey
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="Record"/> by MFN.
        /// </summary>
        public static IComparer<Record> ByMfn()
        {
            return new ByMfnComparer();
        }

        /// <summary>
        /// Compare <see cref="Record"/> by index.
        /// </summary>
        public static IComparer<Record> ByIndex()
        {
            return new ByIndexComparer();
        }

        /// <summary>
        /// Compare <see cref="Record"/> by descrption.
        /// </summary>
        public static IComparer<Record> ByDescription()
        {
            return new ByDescriptionComparer();
        }

        /// <summary>
        /// Compare <see cref="Record"/> by sort key.
        /// </summary>
        public static IComparer<Record> BySortKey()
        {
            return new BySortKeyComparer();
        }

        #endregion

    } // class RecordComparer

} // namespace ManagedIrbis
