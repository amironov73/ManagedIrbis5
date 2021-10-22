// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PeekableEnumeratorExtensions.cs -- расширения для перечислителя с подглядыванием
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

#pragma warning disable 8603
#pragma warning disable 8604

namespace AM.Linq
{
    /// <summary>
    /// Методы расширения для перечислителя с подглядыванием.
    /// </summary>
    public static class PeekableEnumeratorExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static PeekableEnumerator<T> GetPeekableEnumerator<T>(this IEnumerable<T> enumerable)
        {
            return new PeekableEnumerator<T>(enumerable.GetEnumerator());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> TakeWhile<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate)
        {
            // true, true, [break], false
            while (enumerator.TakeIf(predicate, out var value))
            {
                yield return value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> TakeUntil<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate)
        {
            // false, false, [break], true
            while (enumerator.TakeIfNot(predicate, out var value))
            {
                yield return value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="isFirst"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> TakeSliceByFirst<T>(this PeekableEnumerator<T> enumerator, Predicate<T> isFirst)
        {
            // first, last, [break], first
            while (enumerator.MoveNext(out var curr))
            {
                yield return curr;
                if (enumerator.PeekNext(out var next) && isFirst(next)) yield break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="isLast"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> TakeSliceByLast<T>(this PeekableEnumerator<T> enumerator, Predicate<T> isLast)
        {
            // first, last, [break]
            while (enumerator.MoveNext(out var curr))
            {
                yield return curr;
                if (isLast(curr)) yield break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="predicate"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TakeIf<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate,
            [MaybeNullWhen(false)] out T value)
        {
            if (enumerator.PeekNext(out var next) && predicate(next))
            {
                return enumerator.MoveNext(out value);
            }

            value = default;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="predicate"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TakeIfNot<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate,
            [MaybeNullWhen(false)] out T value)
        {
            if (enumerator.PeekNext(out var next) && !predicate(next))
            {
                return enumerator.MoveNext(out value);
            }

            value = default;
            return false;
        }
    }
}
