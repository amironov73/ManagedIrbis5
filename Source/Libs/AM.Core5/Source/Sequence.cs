// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Sequence.cs -- возня вокруг IEnumerable
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Возня вокруг <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class Sequence
    {
        #region Public methods

        /// <summary>
        /// Первый элемент из последовательности либо значение по умолчанию.
        /// </summary>
        public static T? FirstOr<T>
            (
                this IEnumerable<T> list,
                T? defaultValue
            )
            where T: class
        {
            foreach (T item in list)
            {
                return item;
            }

            return defaultValue;
        } // method FirstOr

        /// <summary>
        /// Первый элемент из последовательности либо значение по умолчанию.
        /// </summary>
        public static T? FirstOr<T>
            (
                this IEnumerable<T> list,
                T? defaultValue
            )
            where T: struct
        {
            foreach (T item in list)
            {
                return item;
            }

            return defaultValue;
        } // method FirstOr

        /// <summary>
        /// Порождает последовательность из одного элемента.
        /// </summary>
        [Pure]
        public static IEnumerable<T> FromItem<T>
            (
                T item
            )
        {
            yield return item;

        } // method FromItem

        /// <summary>
        /// Порождает последовательность из двух элементов.
        /// </summary>
        [Pure]
        public static IEnumerable<T> FromItems<T>
            (
                T item1,
                T item2
            )
        {
            yield return item1;
            yield return item2;

        } // method FromItems

        /// <summary>
        /// Порождает последовательность из трех элементов.
        /// </summary>
        [Pure]
        public static IEnumerable<T> FromItems<T>
            (
                T item1,
                T item2,
                T item3
            )
        {
            yield return item1;
            yield return item2;
            yield return item3;

        } // method FromItems

        /// <summary>
        /// Порождает последовательность из перечисленных элементов.
        /// </summary>
        [Pure]
        public static IEnumerable<T?> FromItems<T>
            (
                params T?[] items
            )
        {
            foreach (T? item in items)
            {
                yield return item;
            }

        } // method FromItems

        /// <summary>
        /// Вычисление максимального значения в последовательности.
        /// </summary>
        public static T? MaxOrDefault<T>
            (
                this IEnumerable<T> sequence,
                T? defaultValue
            )
            where T: class
        {
            // TODO: сделать без массива

            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return defaultValue;
            }

            var result = array.Max();

            return result;

        } // method MaxOrDefault

        /// <summary>
        /// Вычисление максимального значения в последовательности.
        /// </summary>
        public static T? MaxOrDefault<T>
            (
                this IEnumerable<T> sequence,
                T? defaultValue
            )
            where T: struct
        {
            // TODO: сделать без массива

            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return defaultValue;
            }

            var result = array.Max();

            return result;

        } // method MaxOrDefault

        /// <summary>
        /// Вычисление максимального значения в последовательности.
        /// </summary>
        public static TOutput? MaxOrDefault<TInput, TOutput>
            (
                this IEnumerable<TInput> sequence,
                Func<TInput, TOutput?> selector,
                TOutput? defaultValue
            )
            where TOutput: class
        {
            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return defaultValue;
            }

            var result = array.Max(selector);

            return result;

        } // method MaxOrDefault

        /// <summary>
        /// Вычисление максимального значения в последовательности.
        /// </summary>
        public static TOutput? MaxOrDefault<TInput, TOutput>
            (
                this IEnumerable<TInput> sequence,
                Func<TInput, TOutput?> selector,
                TOutput? defaultValue
            )
            where TOutput: struct
        {
            var array = sequence.ToArray();
            if (array.Length == 0)
            {
                return defaultValue;
            }

            var result = array.Max(selector);

            return result;

        } // method MaxOrDefault

        /// <summary>
        /// Отбирает из последовательности только
        /// ненулевые элементы.
        /// </summary>
        public static IEnumerable<T> NonNullItems<T>
            (
                this IEnumerable<T?> sequence
            )
            where T : class
        {
            foreach (var item in sequence)
            {
                if (!ReferenceEquals(item, null))
                {
                    yield return item;
                }
            }

        } // method NonNullItems

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        public static IEnumerable<string> NonEmptyLines
            (
                this IEnumerable<string?> sequence
            )
        {
            foreach (var line in sequence)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    yield return line;
                }
            }

        } // method NonEmptyLines

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        public static IEnumerable<ReadOnlyMemory<char>> NonEmptyLines
            (
                this IEnumerable<ReadOnlyMemory<char>> sequence
            )
        {
            foreach (var line in sequence)
            {
                if (!line.IsEmpty)
                {
                    yield return line;
                }
            }
        } // method NonEmptyLines

        /// <summary>
        /// Повторяет указанное значение.
        /// </summary>
        [Pure]
        public static IEnumerable<T> Repeat<T>
            (
                T value,
                int count
            )
        {
            while (count-- > 0)
            {
                yield return value;
            }
        } // method Repeat

        /// <summary>
        /// Repeats the specified list.
        /// </summary>
        public static IEnumerable<T> Repeat<T>
            (
                IEnumerable<T> list,
                int count
            )
        {
            var array = list.ToArray();

            while (count-- > 0)
            {
                foreach (var value in array)
                {
                    yield return value;
                }
            }
        } // method Repeat

        /// <summary>
        /// Заменяет в последовательности одно значение на другое.
        /// </summary>
        public static IEnumerable<T> Replace<T>
            (
                this IEnumerable<T> list,
                T replaceFrom,
                T replaceTo
            )
            where T : IEquatable<T>
        {
            foreach (T item in list)
            {
                if (item.Equals(replaceFrom))
                {
                    yield return replaceTo;
                }
                else
                {
                    yield return item;
                }
            }
        } // method Replace

        /// <summary>
        /// Добавляет некоторое действие к каждому
        /// элементу последовательности.
        /// </summary>
        public static IEnumerable<T> Tee<T>
            (
                this IEnumerable<T> list,
                Action<T> action
            )
        {
            foreach (var item in list)
            {
                action(item);

                yield return item;
            }
        } // method Tee

        /// <summary>
        /// Добавляет некоторое действие к каждому
        /// элементу последовательности.
        /// </summary>
        public static IEnumerable<T> Tee<T>
            (
                this IEnumerable<T> list,
                Action<int, T> action
            )
        {
            var index = 0;
            foreach (var item in list)
            {
                action(index, item);
                index++;

                yield return item;
            }
        } // method Tee

        /// <summary>
        /// Перемежаем значения некоторым разделителем.
        /// </summary>
        public static IEnumerable<T> Separate<T>
            (
                this IEnumerable<T> sequence,
                T separator
            )
        {
            var first = true;
            foreach (var obj in sequence)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    yield return separator;
                }
                yield return obj;
            }
        } // method Separate

        /// <summary>
        /// Нарезает последовательность на куски (массивы)
        /// не больше указанного размера.
        /// </summary>
        public static IEnumerable<T[]> Chunk<T>
            (
                this IEnumerable<T> sequence,
                int pieceSize
            )
        {
            if (pieceSize <= 0)
            {
                Magna.Error
                    (
                        nameof(Sequence) + "::" + nameof(Chunk)
                        + "pieceSize="
                        + pieceSize
                    );

                throw new ArgumentOutOfRangeException(nameof(pieceSize));
            }

            var piece = new List<T>(pieceSize);
            foreach (T item in sequence)
            {
                piece.Add(item);
                if (piece.Count >= pieceSize)
                {
                    yield return piece.ToArray();
                    piece = new List<T>(pieceSize);
                }
            }

            if (piece.Count != 0)
            {
                yield return piece.ToArray();
            }

        } // method Chunk

        /// <summary>
        /// Get next item from the sequence.
        /// </summary>
        public static T? NetOrDefault<T> (this IEnumerator<T> sequence, T? defaultValue = default) =>
            sequence.MoveNext() ? sequence.Current : defaultValue;

        /// <summary>
        /// Get next item from the sequence.
        /// </summary>
        public static T? NetOrDefault<T> (this IEnumerator sequence, T? defaultValue = default) =>
            sequence.MoveNext() ? (T) sequence.Current : defaultValue;

        #endregion

    } // class Sequence

} // namespace AM
