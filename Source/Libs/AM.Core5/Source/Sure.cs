// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
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
        /// State assertion
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void AssertState
            (
                bool condition,
                string message
            )
        {
            if (!condition)
            {
                throw new InvalidOleVariantTypeException(message);
            }
        }

        /// <summary>
        /// Check whether <paramref name="value"/> is not defined.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Defined<T>
            (
                T value,
                string argumentName
            )
            where T : struct
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Checks whether specified files exists.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FileExists
            (
                string? path,
                string argumentName
            )
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(argumentName);
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException
                    (
                        argumentName
                        + " : "
                        + path
                    );
            }
        }

        /// <summary>
        /// Checks whether <paramref name="argument"/> fits into
        /// specified range <paramref name="fromValue"/>
        /// to <paramref name="toValue"/>.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void InRange
            (
                int argument,
                int fromValue,
                int toValue,
                string argumentName
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Checks whether <paramref name="argument"/> fits into
        /// specified range <paramref name="fromValue"/>
        /// to <paramref name="toValue"/>.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void InRange
            (
                long argument,
                long fromValue,
                long toValue,
                string argumentName
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Checks whether <paramref name="argument"/> fits into
        /// specified range <paramref name="fromValue"/>
        /// to <paramref name="toValue"/>.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InRange
            (
                double argument,
                double fromValue,
                double toValue,
                string argumentName
            )
        {
            if (argument < fromValue
                || argument > toValue)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NonNegative
            (
                int argument,
                string argumentName
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NonNegative
            (
                long argument,
                string argumentName
            )
        {
            if (argument < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is not negative.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NonNegative
            (
                double argument,
                string argumentName
            )
        {
            if (argument < 0.0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="argument" /> != <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>
            (
                T? argument,
                string argumentName
            )
            where T : class
        {
            if (ReferenceEquals(argument, null))
            {
                throw new ArgumentException(argumentName);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="argument" /> != <c>null</c>.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNull<T>
            (
                T? argument,
                string argumentName
            )
            where T : struct
        {
            if (!argument.HasValue)
            {
                throw new ArgumentException(argumentName);
            }
        }

        /// <summary>
        /// Ensures that <paramref name="argument" />
        /// is not null nor empty.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void NotNullNorEmpty
            (
                string? argument,
                string argumentName
            )
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is positive.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Positive
            (
                int argument,
                string argumentName
            )
        {
            if (argument <= 0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is positive.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Positive
            (
                long argument,
                string argumentName
            )
        {
            if (argument <= 0.0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        /// <summary>
        /// Check whether <paramref name="argument"/> is positive.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void Positive
            (
                double argument,
                string argumentName
            )
        {
            if (argument <= 0.0)
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        #endregion
    }
}
