// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DispatcherStream.cs -- ввод-вывод, синхронизированный в UI-потоком
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;

using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Threading;

/// <summary>
/// Ввод-вывод, синхронизированный с UI-потоком.
/// </summary>
public sealed class DispatcherStream
    : Stream
{
    #region Properties

    /// <summary>
    /// Синхронизируемый ввод-вывод.
    /// </summary>
    public Stream InnerStream { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DispatcherStream
        (
            Stream innerStream
        )
    {
        Sure.NotNull (innerStream);

        InnerStream = innerStream;
    }

    #endregion

    #region Stream members

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            InnerStream.Flush();
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => InnerStream.Flush()
                )
                .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.Read(byte[],int,int)"/>
    public override int Read
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (offset, count, buffer);

        if (Dispatcher.UIThread.CheckAccess())
        {
            return InnerStream.Read (buffer, offset, count);
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => InnerStream.Read (buffer, offset, count)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="Stream.Read(System.Span{byte})"/>
    public override int Read
        (
            Span<byte> buffer
        )
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            return InnerStream.Read (buffer);
        }

        var array = ArrayPool<byte>.Shared.Rent (buffer.Length);
        var result = Dispatcher.UIThread.InvokeAsync
            (
                () => InnerStream.Read (array, 0, array.Length)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();

        array.AsSpan (0, result).CopyTo (buffer);
        ArrayPool<byte>.Shared.Return (array);

        return result;
    }

    /// <inheritdoc cref="Stream.ReadByte"/>
    public override int ReadByte()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return InnerStream.ReadByte();
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => InnerStream.ReadByte()
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="Stream.Seek"/>
    public override long Seek
        (
            long offset,
            SeekOrigin origin
        )
    {
        Sure.Defined (origin);

        if (Dispatcher.UIThread.CheckAccess())
        {
            return InnerStream.Seek (offset, origin);
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => InnerStream.Seek (offset, origin)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="Stream.SetLength"/>
    public override void SetLength
        (
            long value
        )
    {
        Sure.NonNegative (value);

        if (Dispatcher.UIThread.CheckAccess())
        {
            InnerStream.SetLength (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => InnerStream.SetLength (value)
                )
                .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.Write(byte[],int,int)"/>
    public override void Write
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (offset, count, buffer);

        if (count == 0)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            InnerStream.Write (buffer, offset, count);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => InnerStream.Write (buffer, offset, count)
                )
                .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.Write(System.ReadOnlySpan{byte})"/>
    public override void Write
        (
            ReadOnlySpan<byte> buffer
        )
    {
        if (buffer.IsEmpty)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            InnerStream.Write (buffer);
        }
        else
        {
            var array = ArrayPool<byte>.Shared.Rent (buffer.Length);
            buffer.CopyTo (array);
            Dispatcher.UIThread.InvokeAsync
                (
                    () => InnerStream.Write (array, 0, array.Length)
                )
                .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.WriteByte"/>
    public override void WriteByte
        (
            byte value
        )
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            InnerStream.WriteByte (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => InnerStream.WriteByte (value)
                )
                .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead
    {
        get
        {
            return Dispatcher.UIThread.CheckAccess()
                ? InnerStream.CanRead
                : Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.CanRead
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek
    {
        get
        {
            return Dispatcher.UIThread.CheckAccess()
                ? InnerStream.CanSeek
                : Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.CanSeek
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite
    {
        get
        {
            return Dispatcher.UIThread.CheckAccess()
                ? InnerStream.CanWrite
                : Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.CanWrite
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Stream.Length"/>
    public override long Length
    {
        get
        {
            return Dispatcher.UIThread.CheckAccess()
                ? InnerStream.Length
                : Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.Length
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
        }
    }

    /// <inheritdoc cref="Position"/>
    public override long Position
    {
        get
        {
            return Dispatcher.UIThread.CheckAccess()
                ? InnerStream.Position
                : Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.Position
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
        }

        set
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                InnerStream.Position = value;
            }
            else
            {
                Dispatcher.UIThread.InvokeAsync
                    (
                        () => InnerStream.Position = value
                    )
                    .ConfigureAwait (false).GetAwaiter().GetResult();
            }
        }
    }

    #endregion
}
