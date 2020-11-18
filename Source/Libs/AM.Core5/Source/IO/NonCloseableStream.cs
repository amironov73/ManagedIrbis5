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

/* NonCloseableStream.cs -- незакрываемый Stream
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Незакрываемый <see cref="Stream"/>.
    /// Для закрытия надо вызывать
    /// <see cref="M:AM.IO.NonCloseableStream.ReallyClose"/>.
    /// </summary>
    public class NonCloseableStream
        : Stream,
          IDisposable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NonCloseableStream"/> class.
        /// </summary>
        public NonCloseableStream (Stream innerStream)
            => _innerStream = innerStream;

        #endregion

        #region Private members

        private readonly Stream _innerStream;

        #endregion

        #region Public methods

        /// <summary>
        /// Really closes the stream.
        /// </summary>
        public virtual void ReallyClose()
        {
            _innerStream.Dispose();
        }

        #endregion

        #region Stream members

        /// <inheritdoc cref="Stream.CanRead" />
        public override bool CanRead => _innerStream.CanRead;

        /// <inheritdoc cref="Stream.CanSeek" />
        public override bool CanSeek => _innerStream.CanSeek;

        /// <inheritdoc cref="Stream.CanWrite" />
        public override bool CanWrite => _innerStream.CanWrite;

        /// <summary>
        /// NOT closes the current stream and releases any resources
        /// (such as sockets and file handles) associated with the current stream.
        /// </summary>
        /// <seealso cref="M:AM.IO.NonCloseable.NonCloseableStream.ReallyClose"/>
        public override void Close()
        {
            // Nothing to do actually
        }

        /// <inheritdoc cref="Stream.Flush" />
        public override void Flush() => _innerStream.Flush();

        /// <inheritdoc cref="Stream.Length" />
        public override long Length => _innerStream.Length;

        /// <inheritdoc cref="Stream.Position" />
        public override long Position
        {
            get => _innerStream.Position;
            set => _innerStream.Position = value;
        }

        /// <inheritdoc cref="Stream.Read(byte[],int,int)" />
        public override int Read (byte[] buffer, int offset, int count)
            => _innerStream.Read(buffer, offset, count);

        /// <inheritdoc cref="Stream.Seek" />
        public override long Seek (long offset, SeekOrigin origin)
            => _innerStream.Seek(offset, origin);

        /// <inheritdoc cref="Stream.SetLength" />
        public override void SetLength (long value)
            => _innerStream.SetLength(value);

        /// <inheritdoc cref="Stream.Write(byte[],int,int)" />
        public override void Write (byte[] buffer, int offset, int count)
            => _innerStream.Write(buffer, offset, count);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        void IDisposable.Dispose()
        {
            // Nothing to do actually
        }

        #endregion
    }
}
