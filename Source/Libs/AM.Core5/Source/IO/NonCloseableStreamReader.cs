// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* NonCloseableStreamReader.cs -- незакрываемый StreamReader
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Незакрываемая версия <see cref="StreamReader"/>
    /// </summary>
    public class NonCloseableStreamReader
        : StreamReader,
          IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream
            )
            : base(stream)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path
            )
            : base(path)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                bool detectEncodingFromByteOrderMarks
            )
            : base(stream, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                Encoding encoding
            )
            : base(stream, encoding)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                bool detectEncodingFromByteOrderMarks
            )
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                Encoding encoding
            )
            : base(path, encoding)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                Stream stream,
                Encoding encoding,
                bool detectEncodingFromByteOrderMarks
            )
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonCloseableStreamReader
            (
                string path,
                Encoding encoding,
                bool detectEncodingFromByteOrderMarks
            )
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public NonCloseableStreamReader
            (
                StreamReader reader
            )
            : base(reader.BaseStream, reader.CurrentEncoding)
        {
        }

        #endregion

        #region Public methods

        #endregion

        #region StreamReader members

        /// <inheritdoc cref="StreamReader.Close" />
        public override void Close()
        {
            // Nothing to do actually
        }

        #endregion

        #region IDisposable Members

        /// <inheritdoc cref="IDisposable.Dispose" />
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
