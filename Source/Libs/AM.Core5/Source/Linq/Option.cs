// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Option.cs -- опциональное значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Linq
{
    /// <summary>
    /// Опциональное значение.
    /// </summary>
    /// <remarks>
    /// Заимствовано из https://gist.github.com/Denis535/c492f0458b5751b25bf423b9048b2798
    /// </remarks>
    public static class Option
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="opt1"></param>
        /// <param name="opt2"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Compare<T>(Option<T> opt1, Option<T> opt2)
        {
            if (opt1.HasValue && !opt2.HasValue) return 1;
            if (!opt1.HasValue && opt2.HasValue) return -1;
            return Comparer<T>.Default.Compare(opt1.Value, opt2.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opt1"></param>
        /// <param name="opt2"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Equals<T>(Option<T> opt1, Option<T> opt2)
        {
            if (opt1.HasValue && !opt2.HasValue) return false;
            if (!opt1.HasValue && opt2.HasValue) return false;
            return EqualityComparer<T>.Default.Equals(opt1.Value, opt2.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="optionType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Type? GetUnderlyingType(Type optionType)
        {
            if (optionType is null) throw new ArgumentNullException(nameof(optionType));
            if (GetUnboundType(optionType) == typeof(Option<>)) return optionType.GetGenericArguments().First();

            return null;
        }

        private static Type GetUnboundType(Type type)
        {
            if (type.IsGenericType)
            {
                return type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
            }
            else
            {
                return type;
            }
        }
    }

    /// <summary>
    /// Опциональное знаение.
    /// </summary>
    [Serializable]
    public readonly struct Option<T>
    {
        private readonly bool hasValue;
        private readonly T value;

        /// <summary>
        ///
        /// </summary>
        public bool HasValue => hasValue;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public T Value => hasValue ? value : throw new InvalidOperationException("Option object must have a value");

        /// <summary>
        ///
        /// </summary>
        public T? ValueOrDefault => hasValue ? value : default;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public Option(T value)
        {
            this.hasValue = true;
            this.value = value;
        }

        // GetValueOrDefault
        /// <summary>
        ///
        /// </summary>
        /// <param name="default"></param>
        /// <returns></returns>
        public T? GetValueOrDefault(T? @default)
        {
            return hasValue ? value : @default;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return value?.ToString() ?? "";
        }

        /// <inheritdoc />
        public override bool Equals(object? other)
        {
            if (value is null) return other is null;
            if (other is null) return false;
            return value.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return value?.GetHashCode() ?? 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator T(Option<T> value)
        {
            return value.Value;
        }
    }
}
