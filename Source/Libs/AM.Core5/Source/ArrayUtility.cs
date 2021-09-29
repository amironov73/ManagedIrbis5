// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ArrayUtility.cs -- утилиты для работы с массивами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Утилиты для работы с массивами <see cref="Array"/>.
    /// </summary>
    public static class ArrayUtility
    {
        #region Public methods

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="TFrom">Type of source array.</typeparam>
        /// <typeparam name="TTo">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        public static TTo[] ChangeType<TFrom, TTo>
            (
                TFrom[] sourceArray
            )
        {
            var result = new TTo[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Changes type of given array to the specified type.
        /// </summary>
        /// <param name="sourceArray">Source array.</param>
        /// <typeparam name="TTo">Type of destination array.</typeparam>
        /// <returns>Allocated array with converted items.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sourceArray"/> is <c>null</c>.
        /// </exception>
        public static TTo[] ChangeType<TTo>
            (
                Array sourceArray
            )
        {
            var result = new TTo[sourceArray.Length];
            Array.Copy(sourceArray, result, sourceArray.Length);

            return result;
        }

        /// <summary>
        /// Clone the array.
        /// </summary>
        public static T[] Clone<T>
            (
                T[] array
            )
            where T: ICloneable
        {
            var result = (T[]) array.Clone();
            for (var i = 0; i < array.Length; i++)
            {
                result[i] = (T) array[i].Clone();
            }

            return result;
        }

        /// <summary>
        /// Whether segment of first array
        /// coincides with segment of second array.
        /// </summary>
        public static bool Coincide<T>
            (
                T[] firstArray,
                int firstOffset,
                T[] secondArray,
                int secondOffset,
                int length
            )
            where T: IEquatable<T>
        {
            Sure.NonNegative(firstOffset, nameof(firstOffset));
            Sure.NonNegative(secondOffset, nameof(secondOffset));
            Sure.NonNegative(length, nameof(length));

            // Совпадают ли два куска массивов?
            // Куски нулевой длины считаются совпадающими.

            for (var i = 0; i < length; i++)
            {
                var first = firstArray[firstOffset + i];
                var second = secondArray[secondOffset + i];
                if (!first.Equals(second))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two specified arrays by elements.
        /// </summary>
        /// <param name="firstArray">First array to compare.</param>
        /// <param name="secondArray">Second array to compare.</param>
        /// <returns><para>Less than zero - first array is less.</para>
        /// <para>Zero - arrays are equal.</para>
        /// <para>Greater than zero - first array is greater.</para>
        /// </returns>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="firstArray"/> or
        /// <paramref name="secondArray"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">Length of
        /// <paramref name="firstArray"/> is not equal to length of
        /// <paramref name="secondArray"/>.
        /// </exception>
        public static int Compare<T>
            (
                T[] firstArray,
                T[] secondArray
            )
            where T : IComparable<T>
        {
            if (firstArray.Length != secondArray.Length)
            {
                Magna.Error
                    (
                        nameof(ArrayUtility)
                        + "::"
                        + nameof(Compare)
                        + ": length not equal"
                    );

                throw new ArgumentException();
            }

            for (var i = 0; i < firstArray.Length; i++)
            {
                var result = firstArray[i].CompareTo(secondArray[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts the specified array.
        /// </summary>
        public static TTo[] Convert<TFrom, TTo>
            (
                TFrom[] array
            )
        {
            var result = new TTo[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                result[i] = Utility.ConvertTo<TTo>(array[i]);
            }

            return result;
        }

        /// <summary>
        /// Creates the array of specified length initializing it with
        /// specified value.
        /// </summary>
        /// <param name="length">Desired length of the array.</param>
        /// <param name="initialValue">The initial value of
        /// array items.</param>
        /// <returns>Created and initialized array.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        public static T[] Create<T>
            (
                int length,
                T initialValue
            )
        {
            Sure.NonNegative(length, nameof(length));

            var result = new T[length];
            for (var i = 0; i < length; i++)
            {
                result[i] = initialValue;
            }

            return result;
        }

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <remarks>
        /// Возможна отрицательная нумерация
        /// (означает индекс с конца массива).
        /// При выходе за границы массива
        /// выдаётся значение по умолчанию.
        /// </remarks>
        public static T? GetOccurrence<T>
            (
                this T[] array,
                int occurrence
            )
        {
            var length = array.Length;

            occurrence = occurrence >= 0
                ? occurrence
                : length + occurrence;

            var result = default(T);

            if (length != 0
                && occurrence >= 0
                && occurrence < length)
            {
                result = array[occurrence];
            }

            return result;

        } // method GetOccurrence

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <remarks>
        /// Возможна отрицательная нумерация
        /// (означает индекс с конца массива).
        /// При выходе за границы массива
        /// выдаётся значение по умолчанию.
        /// </remarks>
        public static T GetOccurrence<T>
            (
                this T[] array,
                int occurrence,
                T defaultValue
            )
        {
            var length = array.Length;

            occurrence = occurrence >= 0
                ? occurrence
                : length + occurrence;

            var result = defaultValue;

            if (length != 0
                && occurrence >= 0
                && occurrence < length)
            {
                result = array[occurrence];
            }

            return result;
        }

        /// <summary>
        /// Извлечение непрерывного диапазона элементов массива.
        /// </summary>
        public static T[] GetSpan<T>
            (
                this T[] array,
                int offset,
                int count
            )
        {
            Sure.NonNegative (offset, nameof (offset));
            Sure.NonNegative (count, nameof (count));

            if (offset > array.Length)
            {
                return Array.Empty<T>();
            }

            if (offset + count > array.Length)
            {
                count = array.Length - offset;
            }

            if (count <= 0)
            {
                return Array.Empty<T>();
            }

            var result = new T[count];
            Array.Copy (array, offset, result, 0, count);

            return result;

        } // method GetSpan

        /// <summary>
        /// Get span of the array.
        /// </summary>
        public static T[] GetSpan<T>
            (
                this T[] array,
                int offset
            )
        {
            Sure.NonNegative(offset, nameof(offset));

            if (offset >= array.Length)
            {
                return Array.Empty<T>();
            }

            var count = array.Length - offset;
            T[] result = array.GetSpan(offset, count);

            return result;
        }

        /// <summary>
        /// Determines whether the specified array is null or empty
        /// (has zero length).
        /// </summary>
        /// <param name="array">Array to check.</param>
        /// <returns><c>true</c> if the array is null or empty;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty
            (
                Array? array
            )
        {
            return ReferenceEquals(array, null) || array.Length == 0;
        }

        /// <summary>
        /// Merges the specified arrays.
        /// </summary>
        /// <param name="arrays">Arrays to merge.</param>
        /// <returns>Array that consists of all <paramref name="arrays"/>
        /// items.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// At least one of <paramref name="arrays"/> is <c>null</c>.
        /// </exception>
        public static T[] Merge<T>
            (
                params T[]?[] arrays
            )
        {
            var resultLength = 0;
            for (var i = 0; i < arrays.Length; i++)
            {
                var item = arrays[i];
                if (ReferenceEquals (item, null))
                {
                    Magna.Error
                        (
                            nameof (ArrayUtility)
                            + "::"
                            + nameof (Merge)
                            + ": array["
                            + i
                            + "] is null"
                        );

                    throw new ArgumentNullException (nameof (arrays));
                }

                resultLength += item.Length;

            } // for

            var result = new T[resultLength];
            var offset = 0;
            foreach (var item in arrays)
            {
                item!.CopyTo (result, offset);
                offset += item.Length;
            }

            return result;

        } // method Merge

        /// <summary>
        /// Безопасное вычисление длины массива.
        /// </summary>
        public static int SafeLength<T> (this T[]? array) => array?.Length ?? 0;

        /// <summary>
        /// Разбиение массива на (почти) равные части.
        /// </summary>
        public static T[][] SplitArray<T>
            (
                T[] array,
                int partCount
            )
        {
            Sure.Positive(partCount, nameof(partCount));

            var result = new List<T[]>(partCount);
            var length = array.Length;
            var chunkSize = length / partCount;
            while (chunkSize * partCount < length)
            {
                chunkSize++;
            }
            var offset = 0;
            for (var i = 0; i < partCount; i++)
            {
                var size = Math.Min(chunkSize, length - offset);
                var chunk = new T[size];
                Array.Copy
                    (
                        array,
                        offset,
                        chunk,
                        0,
                        size
                    );
                result.Add(chunk);
                offset += size;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts to string array using
        /// <see cref="object.ToString"/> method.
        /// </summary>
        public static string[] ToString<T>
            (
                T[] array
            )
        {
            var result = new string[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                var o = array[i];
                if (ReferenceEquals(o, null))
                {
                    result[i] = "(null)";
                }
                else
                {
                    result[i] = o.ToString()!;
                }
            }

            return result;
        }

        #endregion
    }
}
