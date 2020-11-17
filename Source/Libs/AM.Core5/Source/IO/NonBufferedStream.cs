// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* NonBufferedStream.cs -- небуферизующий поток
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using AM.Collections;
using AM.Runtime;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Небуферизующий поток.
    /// Просто вызывает <see cref="Stream.Flush"/>
    /// после каждой операции вывода.
    /// </summary>
    public sealed class NonBufferedStream
        : Stream
    {
        #region Properties

        /// <summary>
        /// Inner stream.
        /// </summary>
        public Stream InnerStream { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NonBufferedStream
            (
                Stream innerStream
            )
        {
            InnerStream = innerStream;
        }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush()
        {
            InnerStream.Flush();
        }

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek
            (
                long offset,
                SeekOrigin origin
            )
        {
            return InnerStream.Seek(offset, origin);
        }

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength
            (
                long value
            )
        {
            Flush();
            InnerStream.SetLength(value);
        }

        /// <inheritdoc cref="Stream.Read(byte[],int,int)" />
        public override int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Flush();

            return InnerStream.Read(buffer, offset, count);
        }

        /// <inheritdoc cref="Stream.Write(byte[],int,int)" />
        public override void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            InnerStream.Write(buffer, offset, count);
            Flush();
        }

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead => InnerStream.CanRead;

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek => InnerStream.CanSeek;

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite => InnerStream.CanWrite;

        /// <inheritdoc cref="Stream.Length" />
        public override long Length => InnerStream.Length;

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get => InnerStream.Position;
            set => InnerStream.Position = value;
        }

        /// <inheritdoc cref="Stream.Close" />
        public override void Close()
        {
            InnerStream.Close();
        }

        /// <inheritdoc cref="Stream.Dispose(bool)" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            InnerStream.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return InnerStream.ToVisibleString();
        }

        #endregion
    }
}
