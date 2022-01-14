// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DelegatedStream.cs -- поток, делегирующий реализацию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Поток, делегирующий реализацию, что позволяет точечно менять поведение.
/// Делегируются только синхронные методы.
/// </summary>
public sealed class DelegatedStream
    : Stream
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DelegatedStream
        (
            Stream inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;

        CanReadDelegate = DefaultCanRead;
        CanSeekDelegate = DefaultCanSeek;
        CanTimeoutDelegate = DefaultCanTimeout;
        CanWriteDelegate = DefaultCanWrite;
        GetLengthDelegate = DefaultGetLength;
        SetLengthDelegate = DefaultSetLength;
        GetPositionDelegate = DefaultGetPosition;
        SetPositionDelegate = DefaultSetPosition;
        GetReadTimeoutDelegate = DefaultGetReadTimeout;
        SetReadTimeoutDelegate = DefaultSetReadTimeout;
        GetWriteTimeoutDelegate = DefaultGetWriteTimeout;
        SetWriteTimeoutDelegate = DefaultSetWriteTimeout;
        FlushDelegate = DefaultFlush;
        ReadDelegate = DefaultRead;
        ReadByteDelegate = DefaultReadByte;
        SeekDelegate = DefaultSeek;
        WriteDelegate = DefaultWrite;
        WriteByteDelegate = DefaultWriteByte;
        CloseDelegate = DefaultClose;
    }

    #endregion

    #region Private members

    private readonly Stream _inner;

    #endregion

    #region Delegates

    /// <inheritdoc cref="Stream.CanRead"/>
    public Func<bool> CanReadDelegate;

    /// <inheritdoc cref="Stream.CanSeek"/>
    public Func<bool> CanSeekDelegate;

    /// <inheritdoc cref="Stream.CanTimeout"/>
    public Func<bool> CanTimeoutDelegate;

    /// <inheritdoc cref="Stream.CanWrite"/>
    public Func<bool> CanWriteDelegate;

    /// <inheritdoc cref="Stream.Length"/>
    public Func<long> GetLengthDelegate;

    /// <inheritdoc cref="Stream.SetLength"/>
    public Action<long> SetLengthDelegate;

    /// <inheritdoc cref="Stream.get_Position"/>
    public Func<long> GetPositionDelegate;

    /// <inheritdoc cref="Stream.set_Position"/>
    public Action<long> SetPositionDelegate;

    /// <inheritdoc cref="Stream.get_ReadTimeout"/>
    public Func<int> GetReadTimeoutDelegate;

    /// <inheritdoc cref="Stream.set_ReadTimeout"/>
    public Action<int> SetReadTimeoutDelegate;

    /// <inheritdoc cref="Stream.get_WriteTimeout"/>
    public Func<int> GetWriteTimeoutDelegate;

    /// <inheritdoc cref="Stream.set_WriteTimeout"/>
    public Action<int> SetWriteTimeoutDelegate;

    /// <inheritdoc cref="Stream.Flush"/>
    public Action FlushDelegate;

    /// <inheritdoc cref="Stream.Read(byte[],int,int)"/>
    public Func<byte[], int, int, int> ReadDelegate;

    /// <inheritdoc cref="Stream.ReadByte"/>
    public Func<int> ReadByteDelegate;

    /// <inheritdoc cref="Stream.Seek"/>
    public Func<long, SeekOrigin, long> SeekDelegate;

    /// <inheritdoc cref="Stream.Write(byte[],int,int)"/>
    public Action<byte[], int, int> WriteDelegate;

    /// <inheritdoc cref="Stream.WriteByte"/>
    public Action<byte> WriteByteDelegate;

    /// <inheritdoc cref="Stream.Close"/>
    public Action CloseDelegate;

    #endregion

    #region Default delegates

    /// <inheritdoc cref="Stream.CanRead"/>
    public bool DefaultCanRead() => _inner.CanRead;

    /// <inheritdoc cref="Stream.CanSeek"/>
    public bool DefaultCanSeek() => _inner.CanSeek;

    /// <inheritdoc cref="Stream.CanTimeout"/>
    public bool DefaultCanTimeout() => _inner.CanTimeout;

    /// <inheritdoc cref="Stream.CanWrite"/>
    public bool DefaultCanWrite() => _inner.CanWrite;

    /// <inheritdoc cref="Stream.Length"/>
    public long DefaultGetLength() => _inner.Length;

    /// <inheritdoc cref="Stream.SetLength"/>
    public void DefaultSetLength (long value) => _inner.SetLength (value);

    /// <inheritdoc cref="Stream.get_Position"/>
    public long DefaultGetPosition() => _inner.Position;

    /// <inheritdoc cref="Stream.set_Position"/>
    public void DefaultSetPosition (long value) => _inner.Position = value;

    /// <inheritdoc cref="Stream.get_ReadTimeout"/>
    public int DefaultGetReadTimeout() => _inner.ReadTimeout;

    /// <inheritdoc cref="Stream.set_ReadTimeout"/>
    public void DefaultSetReadTimeout (int value) => _inner.ReadTimeout = value;

    /// <inheritdoc cref="Stream.get_WriteTimeout"/>
    public int DefaultGetWriteTimeout() => _inner.WriteTimeout;

    /// <inheritdoc cref="Stream.set_WriteTimeout"/>
    public void DefaultSetWriteTimeout (int value) => _inner.WriteTimeout = value;

    /// <inheritdoc cref="Stream.Flush"/>
    public void DefaultFlush() => _inner.Flush();

    /// <inheritdoc cref="Stream.Read(byte[],int,int)"/>
    public int DefaultRead
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        return _inner.Read (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.ReadByte"/>
    public int DefaultReadByte()
    {
        return _inner.ReadByte();
    }

    /// <inheritdoc cref="Stream.Seek"/>
    public long DefaultSeek
        (
            long offset,
            SeekOrigin origin
        )
    {
        return _inner.Seek (offset, origin);
    }

    /// <inheritdoc cref="Stream.Write(byte[],int,int)"/>
    public void DefaultWrite
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        _inner.Write (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.WriteByte"/>
    public void DefaultWriteByte
        (
            byte value
        )
    {
        _inner.WriteByte (value);
    }

    /// <inheritdoc cref="Stream.Close"/>
    public void DefaultClose()
    {
        _inner.Close();
    }

    #endregion

    #region Stream members

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead => CanReadDelegate();

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek => CanSeekDelegate();

    /// <inheritdoc cref="Stream.CanTimeout"/>
    public override bool CanTimeout => CanTimeoutDelegate();

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite => CanWriteDelegate();

    /// <inheritdoc cref="Stream.Length"/>
    public override long Length => GetLengthDelegate();

    /// <inheritdoc cref="Stream.Position"/>
    public override long Position
    {
        get => GetLengthDelegate();
        set => SetPositionDelegate (value);
    }

    /// <inheritdoc cref="Stream.ReadTimeout"/>
    public override int ReadTimeout
    {
        get => GetReadTimeoutDelegate();
        set => SetReadTimeoutDelegate (value);

    }

    /// <inheritdoc cref="Stream.WriteTimeout"/>
    public override int WriteTimeout
    {
        get => GetWriteTimeoutDelegate();
        set => SetWriteTimeoutDelegate (value);

    }

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush()
    {
        FlushDelegate();
    }

    /// <inheritdoc cref="Stream.Read(byte[],int,int)"/>
    public override int Read
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        return ReadDelegate (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.ReadByte"/>
    public override int ReadByte()
    {
        return ReadByteDelegate();
    }

    /// <inheritdoc cref="Stream.Seek"/>
    public override long Seek
        (
            long offset,
            SeekOrigin origin
        )
    {
        return SeekDelegate (offset, origin);
    }

    /// <inheritdoc cref="Stream.SetLength"/>
    public override void SetLength
        (
            long value
        )
    {
        SetLengthDelegate (value);
    }

    /// <inheritdoc cref="Stream.Write(byte[],int,int)"/>
    public override void Write
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        WriteDelegate (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.WriteByte"/>
    public override void WriteByte
        (
            byte value
        )
    {
        WriteByteDelegate (value);
    }

    /// <inheritdoc cref="Stream.Close"/>
    public override void Close()
    {
        CloseDelegate();
    }

    /// <inheritdoc cref="Stream.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        disposing.NotUsed();
        CloseDelegate();
    }

    #endregion
}
