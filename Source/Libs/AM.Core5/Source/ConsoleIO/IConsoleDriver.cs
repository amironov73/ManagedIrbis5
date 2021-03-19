// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* IConsoleDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.ConsoleIO
{
    /// <summary>
    ///
    /// </summary>
    public interface IConsoleDriver
    {
        #region Properties

        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets a value indicating whether a key press
        /// is available in the input stream.
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Gets or sets the title to display
        /// in the console title bar.
        /// </summary>
        string Title { get; set; }


        #endregion

        #region Public methods

        /// <summary>
        /// Clear the console.
        /// </summary>
        void Clear();

        /// <summary>
        /// Read one key.
        /// </summary>
        ConsoleKeyInfo ReadKey
            (
                bool intercept
            );

        /// <summary>
        /// Reads the next character from the standard input stream.
        /// </summary>
        int Read();

        /// <summary>
        /// Read one line.
        /// </summary>
        string? ReadLine();

        /// <summary>
        /// Write text.
        /// </summary>
        void Write
            (
                string? text
            );

        /// <summary>
        /// Goto next line.
        /// </summary>
        void WriteLine();

        #endregion

    } // interface IConsoleDriver

} // namespace AM.ConsoleIO
