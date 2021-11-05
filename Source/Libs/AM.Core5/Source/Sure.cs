// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Sure.cs -- ассерты на все случаи жизни
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Ассерты на все случаи жизни.
    /// </summary>
    public static class Sure
    {
        #region Public methods

        /// <summary>
        /// Проверка состояния объекта.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void AssertState
            (
                bool condition,
                [CallerArgumentExpression("condition")] string? message = null
            )
        {
            if (!condition)
            {
                throw new InvalidOleVariantTypeException (message);
            }

        } // method AssertState

        /// <summary>
        /// Проверка, определено ли значение <paramref name="value"/> в перечислении.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void Defined<T>
            (
                T value,
                [CallerArgumentExpression("value")] string? argumentName = null
            )
            where T : struct
        {
            if (!Enum.IsDefined (typeof (T), value))
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method Defined

        /// <summary>
        /// Проверка существования файла с указанным именем.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void FileExists
            (
                string? path,
                [CallerArgumentExpression("path")] string? argumentName = null
            )
        {
            if (string.IsNullOrEmpty (path))
            {
                throw new ArgumentNullException (argumentName);
            }

            if (!File.Exists (path))
            {
                throw new FileNotFoundException
                    (
                        argumentName
                        + " : "
                        + path
                    );
            }

        } // method FileExists

        /// <summary>
        /// Проверка, попадает ли  <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void InRange
            (
                int argument,
                int fromValue,
                int toValue,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method InRange

        /// <summary>
        /// Проверка, попадает ли <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void InRange
            (
                long argument,
                long fromValue,
                long toValue,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method InRange

        /// <summary>
        /// Проврека, попадает ли <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void InRange
            (
                double argument,
                double fromValue,
                double toValue,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method InRange

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NonNegative
            (
                int argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method NonNegative

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NonNegative
            (
                long argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method NonNegative

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NonNegative
            (
                double argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument < 0.0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method NonNegative

        /// <summary>
        /// Проверка, что указатель <paramref name="argument" /> не <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NotNull<T>
            (
                T? argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
            where T : class
        {
            if (ReferenceEquals (argument, null))
            {
                throw new ArgumentNullException (argumentName);
            }

        } // method NotNull

        /// <summary>
        /// Проверка, что указатель <paramref name="argument" /> не <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NotNull<T>
            (
                T? argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
            where T : struct
        {
            if (!argument.HasValue)
            {
                throw new ArgumentException (argumentName);
            }

        } // method NotNull

        /// <summary>
        /// Проверка, что строка <paramref name="argument" />
        /// не <c>null</c> и не пустая.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void NotNullNorEmpty
            (
                string? argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (string.IsNullOrEmpty (argument))
            {
                throw new ArgumentNullException (argumentName);
            }

        } // method NotNullNorEmpty

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void Positive
            (
                int argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method Positive

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void Positive
            (
                long argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument <= 0.0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method Positive

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        [Conditional ("DEBUG")]
        public static void Positive
            (
                double argument,
                [CallerArgumentExpression("argument")] string? argumentName = null
            )
        {
            if (argument <= 0.0)
            {
                throw new ArgumentOutOfRangeException (argumentName);
            }

        } // method Positive

        #endregion

    } // class Sure

} // namespace AM
