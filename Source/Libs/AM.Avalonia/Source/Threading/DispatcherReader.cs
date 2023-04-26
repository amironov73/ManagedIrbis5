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

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Threading;

/// <summary>
/// Чтение текста, синхронизированное с UI-потоком.
/// </summary>
[PublicAPI]
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
        return Dispatcher.UIThread.CheckAccess()
            ? Reader.Peek()
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.Peek()
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
    }

    /// <inheritdoc cref="TextReader.Read()"/>
    public override int Read()
    {
        return Dispatcher.UIThread.CheckAccess()
            ? Reader.Read()
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.Read()
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
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

        return Dispatcher.UIThread.CheckAccess()
            ? Reader.Read (buffer, index, count)
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.Read (buffer, index, count)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
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
            .GetTask()
            .ConfigureAwait (false)
            .GetAwaiter()
            .GetResult();

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

        return Dispatcher.UIThread.CheckAccess()
            ? Reader.ReadBlock (buffer, index, count)
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.ReadBlock (buffer, index, count)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
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
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();

        array.AsSpan (0, result).CopyTo (buffer);
        ArrayPool<char>.Shared.Return (array);

        return result;
    }

    /// <inheritdoc cref="TextReader.ReadLine"/>
    public override string? ReadLine()
    {
        return Dispatcher.UIThread.CheckAccess()
            ? Reader.ReadLine()
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.ReadLine()
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
    }

    /// <inheritdoc cref="TextReader.ReadToEnd"/>
    public override string ReadToEnd()
    {
        return Dispatcher.UIThread.CheckAccess()
            ? Reader.ReadToEnd()
            : Dispatcher.UIThread.InvokeAsync
                (
                    () => Reader.ReadToEnd()
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
    }

    #endregion
}
