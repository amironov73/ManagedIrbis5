// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* OutputWriter.cs -- wrapper for AbstractOutput
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Text.Output
{
    /// <summary>
    /// Wrapper for <see cref="AbstractOutput"/>.
    /// </summary>
    public sealed class OutputWriter
        : TextWriter
    {
        #region Properties

        /// <summary>
        /// Inner <see cref="AbstractOutput"/>.
        /// </summary>
        public AbstractOutput Output
        {
            get { return _output; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public OutputWriter
            (
                AbstractOutput output
            )
        {
            _output = output;
        }

        #endregion

        #region Private members

        private readonly AbstractOutput _output;

        #endregion

        #region Public methods

        #endregion

        #region TextWriter members

        /// <summary>
        /// When overridden in a derived class,
        /// returns the character encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        /// <summary>
        /// Writes a line terminator to the text string or stream.
        /// </summary>
        public override void WriteLine()
        {
            Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes a string to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        public override void Write(string value)
        {
            Output.Write(value);
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the text string or stream.
        /// </summary>
        /// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written.</param>
        public override void WriteLine(string value)
        {
            Output.WriteLine(value);
        }

        /// <summary>
        /// Writes a character to the text string or stream.
        /// </summary>
        /// <param name="value">The character to write to the text stream.</param>
        public override void Write(char value)
        {
            Write(new string(value, 1));
        }

        /// <summary>
        /// Writes a character array to the text string or stream.
        /// </summary>
        /// <param name="buffer">The character array to write to the text stream.</param>
        public override void Write(char[] buffer)
        {
            Write(new string(buffer));
        }

        /// <summary>
        /// Writes a subarray of characters to the text string or stream.
        /// </summary>
        /// <param name="buffer">The character array to write data from.</param>
        /// <param name="index">The character position in the buffer at which to start retrieving data.</param>
        /// <param name="count">The number of characters to write.</param>
        public override void Write(char[] buffer, int index, int count)
        {
            Write(new string(buffer, index, count));
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="TextWriter.Dispose(bool)"/>
        protected override void Dispose
            (
                bool disposing
            )
        {
            Output.Dispose();
        }

        #endregion
    }
}
