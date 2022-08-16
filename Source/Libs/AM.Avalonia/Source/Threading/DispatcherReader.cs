// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DispatcherReader.cs -- чтение текста, синхронизованное с UI-потоком
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
/// Чтение текста, синхронизированное с UI-потоком.
/// </summary>
public sealed class DispatcherReader
    : TextReader
{
    #region Properties

    /// <summary>
    /// Синхронизимуемый <see cref="TextReader"/>.
    /// </summary>
    public TextReader Reader { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DispatcherReader
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        Reader = reader;
    }

    #endregion

    #region TextReader members

    /// <inheritdoc cref="TextReader.Peek"/>
    public override int Peek()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.Peek();
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.Peek()
            )
        .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="TextReader.Read()"/>
    public override int Read()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.Read();
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.Read()
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="TextReader.Read(char[],int,int)"/>
    public override int Read
        (
            char[] buffer,
            int index,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (index, count, buffer);

        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.Read (buffer, index, count);
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.Read (buffer, index, count)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="TextReader.Read(System.Span{char})"/>
    public override int Read
        (
            Span<char> buffer
        )
    {
        if (buffer.Length == 0)
        {
            return 0;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.Read (buffer);
        }

        var array = ArrayPool<char>.Shared.Rent (buffer.Length);
        var result = Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.Read (array)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();

        array.AsSpan (0, result).CopyTo (buffer);
        ArrayPool<char>.Shared.Return (array);

        return result;
    }

    /// <inheritdoc cref="TextReader.ReadBlock(char[],int,int)"/>
    public override int ReadBlock
        (
            char[] buffer,
            int index,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (index, count, buffer);

        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.ReadBlock (buffer, index, count);
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.ReadBlock (buffer, index, count)
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="TextReader.ReadBlock(System.Span{char})"/>
    public override int ReadBlock
        (
            Span<char> buffer
        )
    {
        if (buffer.Length == 0)
        {
            return 0;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.ReadBlock (buffer);
        }

        var array = ArrayPool<char>.Shared.Rent (buffer.Length);
        var result = Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.ReadBlock (array)
                )
            .ConfigureAwait (false).GetAwaiter().GetResult();

        array.AsSpan (0, result).CopyTo (buffer);
        ArrayPool<char>.Shared.Return (array);

        return result;
    }

    /// <inheritdoc cref="TextReader.ReadLine"/>
    public override string? ReadLine()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.ReadLine();
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.ReadLine()
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    /// <inheritdoc cref="TextReader.ReadToEnd"/>
    public override string ReadToEnd()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            return Reader.ReadToEnd();
        }

        return Dispatcher.UIThread.InvokeAsync
            (
                () => Reader.ReadToEnd()
            )
            .ConfigureAwait (false).GetAwaiter().GetResult();
    }

    #endregion
}
