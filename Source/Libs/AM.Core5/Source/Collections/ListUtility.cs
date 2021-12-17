// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Listtility.cs -- работа со списками List
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Работа со списками <see cref="List{T}"/> и массивами.
    /// </summary>
    /// <remarks>Borrowed from Json.NET.</remarks>
    public static class ListUtility
    {
        #region Public methods

        /// <summary>
        /// Add to list if don't have yet.
        /// </summary>
        public static bool AddDistinct<T>
            (
                this IList<T> list,
                T value
            )
        {
            return list.AddDistinct
                (
                    value,
                    EqualityComparer<T>.Default
                );
        }

        /// <summary>
        /// Add to list if don't have yet.
        /// </summary>
        public static bool AddDistinct<T>
            (
                this IList<T> list,
                T value,
                IEqualityComparer<T> comparer
            )
        {
            if (list.ContainsValue(value, comparer))
            {
                return false;
            }

            list.Add(value);

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public static bool AddRangeDistinct<T>
            (
                this IList<T> list,
                IEnumerable<T> values,
                IEqualityComparer<T> comparer
            )
        {
            var allAdded = true;
            foreach (var value in values)
            {
                if (!list.AddDistinct(value, comparer))
                {
                    allAdded = false;
                }
            }

            return allAdded;
        }

        /// <summary>
        ///
        /// </summary>
        [Pure]
        public static bool ContainsValue<TSource>
            (
                this IEnumerable<TSource> source,
                TSource value,
                IEqualityComparer<TSource> comparer
            )
        {
            foreach (var local in source)
            {
                if (comparer.Equals(local, value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        public static int IndexOf<T>
            (
                this IEnumerable<T> collection,
                Func<T, bool> predicate
            )
        {
            var index = 0;
            foreach (var value in collection)
            {
                if (predicate(value))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Is the list is <c>null</c> or empty?
        /// </summary>
        [Pure]
        public static bool IsNullOrEmpty<T>
            (
                [NotNullWhen((false))] this IList<T>? list
            )
        {
            if (!ReferenceEquals(list, null))
            {
                return list.Count == 0;
            }

            return true;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the list is <c>null</c> or empty.
        /// </summary>
        public static IList<T> ThrowIfNullOrEmpty<T>
            (
                this IList<T>? list
            )
        {
            if (ReferenceEquals(list, null))
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "list is null"
                    );

                throw new ArgumentNullException();
            }

            if (list.Count == 0)
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "list is empty"
                    );

                throw new ArgumentException();
            }

            return list;

        } // method ThrowIfNullOrEmpty

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the list is <c>null</c> or empty.
        /// </summary>
        [Pure]
        public static IList<T> ThrowIfNullOrEmpty<T>
            (
                this IList<T>? list,
                string message
            )
        {
            if (ReferenceEquals(list, null))
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "list is null"
                    );

                throw new ArgumentNullException(message);
            }

            if (list.Count == 0)
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "list is empty"
                    );

                throw new ArgumentException(message);
            }

            return list;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the array is <c>null</c> or empty.
        /// </summary>
        [Pure]
        public static T[] ThrowIfNullOrEmpty<T>
            (
                this T[]? array
            )
        {
            if (ReferenceEquals(array, null))
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "array is null"
                    );

                throw new ArgumentNullException();
            }

            if (array.Length == 0)
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "array is empty"
                    );

                throw new ArgumentException();
            }

            return array;
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the array is <c>null</c> or empty.
        /// </summary>
        [Pure]
        public static T[] ThrowIfNullOrEmpty<T>
            (
                this T[]? array,
                string message
            )
        {
            if (ReferenceEquals(array, null))
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "array is null"
                    );

                throw new ArgumentNullException(message);
            }

            if (array.Length == 0)
            {
                Magna.Error
                    (
                        nameof(ListUtility)
                        + "::"
                        + nameof(ThrowIfNullOrEmpty)
                        + ": "
                        + "array is empty"
                    );

                throw new ArgumentException(message);
            }

            return array;
        }

        #endregion
    }
}
