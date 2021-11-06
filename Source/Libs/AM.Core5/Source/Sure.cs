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
        public static void AssertState
            (
                bool condition,
                [CallerArgumentExpression ("condition")] string? message = null
            )
        {
            if (!condition)
            {
                if (!string.IsNullOrEmpty (message))
                {
                    // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                    throw new ApplicationException(message);
                }

                throw new ApplicationException();

            } // if

        } // method AssertState

        /// <summary>
        /// Проверка, определено ли значение <paramref name="value"/> в перечислении.
        /// </summary>
        [DebuggerHidden]
        public static void Defined<T>
            (
                T value,
                [CallerArgumentExpression ("value")] string? argumentName = null
            )
            where T : struct
        {
            if (!Enum.IsDefined (typeof (T), value))
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();
            }

        } // method Defined

        /// <summary>
        /// Проверка существования файла с указанным именем.
        /// </summary>
        [DebuggerHidden]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void FileExists
            (
                string? path,
                [CallerArgumentExpression ("path")] string? argumentName = null
            )
        {
            if (string.IsNullOrEmpty (path))
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentNullException (argumentName);
                }

                throw new ArgumentNullException();

            } // if

            if (!File.Exists (path))
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new FileNotFoundException
                        (
                            argumentName
                            + " : "
                            + path
                        );
                }

                throw new FileNotFoundException (path);

            } // if

        } // method FileExists

        /// <summary>
        /// Проверка, попадает ли  <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        public static void InRange
            (
                int argument,
                int fromValue,
                int toValue,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue || argument > toValue)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method InRange

        /// <summary>
        /// Проверка, попадает ли <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        public static void InRange
            (
                long argument,
                long fromValue,
                long toValue,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue || argument > toValue)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method InRange

        /// <summary>
        /// Проврека, попадает ли <paramref name="argument"/> в указанный
        /// диапазон от <paramref name="fromValue"/>
        /// до <paramref name="toValue"/> (включительно).
        /// </summary>
        [DebuggerHidden]
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void InRange
            (
                double argument,
                double fromValue,
                double toValue,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < fromValue || argument > toValue)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method InRange

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        public static void NonNegative
            (
                int argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < 0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method NonNegative

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        public static void NonNegative
            (
                long argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < 0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method NonNegative

        /// <summary>
        /// Проверка, что <paramref name="argument"/> не является отрицательным числом.
        /// </summary>
        [DebuggerHidden]
        public static void NonNegative
            (
                double argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument < 0.0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method NonNegative

        /// <summary>
        /// Проверка, что указатель <paramref name="argument" /> не <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        public static void NotNull<T>
            (
                T? argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
            where T : class
        {
            if (argument is null)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentNullException (argumentName);
                }

                throw new ArgumentNullException();

            } // if

        } // method NotNull

        /// <summary>
        /// Проверка, что указатель <paramref name="argument" /> не <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        public static void NotNull<T>
            (
                T? argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
            where T : struct
        {
            if (!argument.HasValue)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentException (argumentName);
                }

                throw new ArgumentException();

            } // if

        } // method NotNull

        /// <summary>
        /// Проверка, что строка <paramref name="argument" />
        /// не <c>null</c> и не пустая.
        /// </summary>
        [DebuggerHidden]
        public static void NotNullNorEmpty
            (
                string? argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (string.IsNullOrEmpty (argument))
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentNullException (argumentName);
                }

                throw new ArgumentNullException();

            } // if

        } // method NotNullNorEmpty

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        public static void Positive
            (
                int argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument <= 0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method Positive

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        public static void Positive
            (
                long argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument <= 0.0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method Positive

        /// <summary>
        /// Проверка, что число <paramref name="argument"/> положительное.
        /// </summary>
        [DebuggerHidden]
        public static void Positive
            (
                double argument,
                [CallerArgumentExpression ("argument")] string? argumentName = null
            )
        {
            if (argument <= 0.0)
            {
                if (!string.IsNullOrEmpty (argumentName))
                {
                    // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                    throw new ArgumentOutOfRangeException (argumentName);
                }

                throw new ArgumentOutOfRangeException();

            } // if

        } // method Positive

        #endregion

    } // class Sure

} // namespace AM
