// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ConsoleInput.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.ConsoleIO
{
    /// <summary>
    /// Console input.
    /// </summary>
    public static class ConsoleInput
    {
        #region Properties

        /// <summary>
        /// Background color.
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get => Driver.BackgroundColor;
            set => Driver.BackgroundColor = value;
        }

        /// <summary>
        /// Driver for the console.
        /// </summary>
        public static IConsoleDriver Driver => _driver;

        /// <summary>
        /// Foreground color.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => Driver.ForegroundColor;
            set => Driver.ForegroundColor = value;
        }

        /// <summary>
        /// Key available?
        /// </summary>
        public static bool KeyAvailable => Driver.KeyAvailable;

        /// <summary>
        /// Console title.
        /// </summary>
        public static string Title
        {
            get => Driver.Title;
            set => Driver.Title = value;
        }

        #endregion

        #region Private members

        private static IConsoleDriver _driver = new SystemConsole();

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the console.
        /// </summary>
        public static void Clear()
        {
            Driver.Clear();
        }

        /// <summary>
        /// Read one character.
        /// </summary>
        public static int Read()
        {
            return Driver.Read();
        }

        /// <summary>
        /// Read one key.
        /// </summary>
        public static ConsoleKeyInfo ReadKey
            (
                bool intercept
            )
        {
            return Driver.ReadKey(intercept);
        }

        /// <summary>
        /// Read line.
        /// </summary>
        public static string? ReadLine()
        {
            return Driver.ReadLine();
        }

        /// <summary>
        /// Set driver.
        /// </summary>
        public static IConsoleDriver SetDriver
            (
                IConsoleDriver driver
            )
        {
            IConsoleDriver previousDriver = _driver;

            _driver = driver;

            return previousDriver;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public static void Write
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                Driver.Write(text);
            }
        }

        /// <summary>
        /// Goto next line.
        /// </summary>
        public static void WriteLine()
        {
            Driver.WriteLine();
        }

        /// <summary>
        /// Write text and goto next line.
        /// </summary>
        public static void WriteLine
            (
                string text
            )
        {
            Write(text);
            WriteLine();
        }

        #endregion

    } // class ConsoleInput

} // namespace AM.ConsoleIO
