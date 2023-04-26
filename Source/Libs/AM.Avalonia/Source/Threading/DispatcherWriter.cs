// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DispatcherWriter.cs -- вывод текста, синхронизованный с UI-потоком
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Text;

using AM.Collections;

using Avalonia.Threading;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Threading;

/// <summary>
/// Вывод текста, синхронизированный с UI-потоком.
/// </summary>
[PublicAPI]
public sealed class DispatcherWriter
    : TextWriter
{
    #region Properties

    /// <summary>
    /// Синхронизируемый <see cref="TextWriter"/>.
    /// </summary>
    public TextWriter Writer { get; }

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding => Writer.Encoding;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DispatcherWriter
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        Writer = writer;
    }

    #endregion

    #region TextWriter members

    /// <inheritdoc cref="TextWriter.Write(char)"/>
    public override void Write
        (
            char value
        )
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.Write(char[])"/>
    public override void Write
        (
            char[]? buffer
        )
    {
        if (buffer.IsNullOrEmpty())
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (buffer);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (buffer)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.Write(char[],int,int)"/>
    public override void Write
        (
            char[] buffer,
            int index,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (index, count, buffer);

        if (count == 0)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (buffer, index, count);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (buffer, index, count)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.Write(object?)"/>
    public override void Write
        (
            object? value
        )
    {
        if (value is null)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.Write(System.ReadOnlySpan{char})"/>
    public override void Write
        (
            ReadOnlySpan<char> buffer
        )
    {
        if (buffer.IsEmpty)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (buffer);
        }
        else
        {
            var array = ArrayPool<char>.Shared.Rent (buffer.Length);
            buffer.CopyTo (array);
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (array)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();

            ArrayPool<char>.Shared.Return (array);
        }
    }

    /// <inheritdoc cref="TextWriter.Write(string?)"/>
    public override void Write
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.Write(System.Text.StringBuilder?)"/>
    public override void Write
        (
            StringBuilder? value
        )
    {
        if (value is null or { Length: 0 })
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.Write (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.Write (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine()"/>
    public override void WriteLine()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine();
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine()
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char)"/>
    public override void WriteLine
        (
            char value
        )
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char[])"/>
    public override void WriteLine
        (
            char[]? buffer
        )
    {
        if (buffer.IsNullOrEmpty())
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (buffer);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine (buffer)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char[],int,int)"/>
    public override void WriteLine
        (
            char[] buffer,
            int index,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.InRange (index, count, buffer);

        if (count == 0)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (buffer, index, count);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine (buffer, index, count)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(object?)"/>
    public override void WriteLine
        (
            object? value
        )
    {
        if (value is null)
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(string?)"/>
    public override void WriteLine
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine (value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(System.Text.StringBuilder?)"/>
    public override void WriteLine
        (
            StringBuilder? value
        )
    {
        if (value is null or { Length: 0 })
        {
            return;
        }

        if (Dispatcher.UIThread.CheckAccess())
        {
            Writer.WriteLine (value);
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync
                (
                    () => Writer.WriteLine(value)
                )
                .GetTask()
                .ConfigureAwait (false)
                .GetAwaiter()
                .GetResult();
        }
    }

    #endregion
}
