﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* SystemConsole.cs -- драйвер для системной консоли
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.ConsoleIO
{
    /// <summary>
    /// Драйвер для системной консоли.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SystemConsole
        : IConsoleDriver
    {
        #region IConsoleDriver members

        /// <inheritdoc cref="IConsoleDriver.BackgroundColor" />
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <inheritdoc cref="IConsoleDriver.ForegroundColor" />
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <inheritdoc cref="IConsoleDriver.KeyAvailable" />
        public bool KeyAvailable => Console.KeyAvailable;

        /// <inheritdoc cref="IConsoleDriver.Title" />
        public string Title
        {
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Console.Title : string.Empty;
            set
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.Title = value;
                }
            }
        }

        /// <inheritdoc cref="IConsoleDriver.Clear" />
        public void Clear()
        {
            Console.Clear();
        }

        /// <inheritdoc cref="IConsoleDriver.Read" />
        public int Read()
        {
            return Console.Read();
        }

        /// <inheritdoc cref="IConsoleDriver.ReadKey" />
        public ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
            return Console.ReadKey(intercept);
        }

        /// <inheritdoc cref="IConsoleDriver.ReadLine" />
        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        /// <inheritdoc cref="IConsoleDriver.Write" />
        public void Write
            (
                string? text
            )
        {
            Console.Write(text);
        }

        /// <inheritdoc cref="IConsoleDriver.WriteLine" />
        public void WriteLine()
        {
            Console.WriteLine();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Убирает из буфера все (возможно) накопившиеся там нажатия клавиш.
        /// </summary>
        /// <param name="intercept">С возможностью прерывания?</param>
        public static void FlushInputBuffer
            (
                bool intercept = true
            )
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(intercept);

            }
        } // method FlushInputBuffer

        /// <summary>
        /// Задерживает выполнение программы,
        /// пока не будет нажата указанная клавиша.
        /// </summary>
        /// <param name="key">Клавиша.</param>
        public static void PressToContinue
            (
                ConsoleKey key = ConsoleKey.Enter
            )
        {
            if (!Enum.IsDefined(typeof(ConsoleKey), key))
            {
                throw new ApplicationException();
            }

            while (Console.ReadKey(true).Key != key)
            {
                // ReSharper disable RedundantJumpStatement
                continue;
                // ReSharper restore RedundantJumpStatement
            }
        } // method PressToContinue

        #endregion

    } // class SystemConsole

} // namespace AM.ConsoleIO
