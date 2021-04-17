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
        public AbstractOutput Output { get; }

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
            Output = output;
        }

        #endregion

        #region TextWriter members

        /// <inheritdoc cref="TextWriter.Encoding"/>
        public override Encoding Encoding => Encoding.Default;

        /// <inheritdoc cref="TextWriter.WriteLine()"/>
        public override void WriteLine()
        {
            Output.WriteLine(string.Empty);
        }

        /// <inheritdoc cref="TextWriter.Write(string?)"/>
        public override void Write
            (
                string? value
            )
        {
            if (value is not null)
            {
                Output.Write(value);
            }
        }

        /// <inheritdoc cref="TextWriter.WriteLine(string?)"/>
        public override void WriteLine
            (
                string? value
            )
        {
            if (value is not null)
            {
                Output.WriteLine(value);
            }
        }

        /// <inheritdoc cref="TextWriter.Write(char)"/>
        public override void Write(char value)
        {
            Write(new string(value, 1));
        }

        /// <inheritdoc cref="TextWriter.Write(char[])"/>
        public override void Write
            (
                char[]? buffer
            )
        {
            if (buffer is not null)
            {
                Write(new string(buffer));
            }
        }

        /// <inheritdoc cref="TextWriter.Write(char[],int,int)"/>
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
