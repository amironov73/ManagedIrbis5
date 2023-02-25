// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* BufferStringWriter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Buffers.Text;

/// <summary>
/// A <see cref="TextWriter"/> implementation that is backed with <see cref="Utf16ValueStringBuilder"/>.
/// </summary>
/// <remarks>
/// It's important to make sure the writer is always properly disposed.
/// </remarks>
[PublicAPI]
public sealed class ZStringWriter
    : TextWriter
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ZStringWriter()
        : this (CultureInfo.CurrentCulture)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ZStringWriter
        (
            IFormatProvider formatProvider
        )
        : base (formatProvider)
    {
        _builder = BufferString.CreateStringBuilder();
        _isOpen = true;
    }

    #endregion

    #region Private members

    private Utf16ValueStringBuilder _builder;
    private bool _isOpen;
    private UnicodeEncoding? _encoding;

    #endregion

    #region TextWriter members

    /// <summary>
    /// Disposes this instance, operations are no longer allowed.
    /// </summary>
    public override void Close()
    {
        Dispose (true);
    }

    /// <inheritdoc cref="TextWriter.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        _builder.Dispose();
        _isOpen = false;
        base.Dispose (disposing);
    }

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding =>
        _encoding ??= new UnicodeEncoding (false, false);

    /// <inheritdoc cref="TextWriter.Write(char)"/>
    public override void Write
        (
            char value
        )
    {
        AssertNotDisposed();

        _builder.Append (value);
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
        Sure.NonNegative (index);
        Sure.NonNegative (count);

        if (buffer.Length - index < count)
        {
            throw new ArgumentException();
        }

        AssertNotDisposed();

        _builder.Append (buffer, index, count);
    }

    /// <inheritdoc cref="TextWriter.Write(string?)"/>
    public override void Write
        (
            string? value
        )
    {
        AssertNotDisposed();

        if (value != null)
        {
            _builder.Append (value);
        }
    }

    /// <inheritdoc cref="TextWriter.WriteAsync(char)"/>
    public override Task WriteAsync
        (
            char value
        )
    {
        Write (value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteAsync(string?)"/>
    public override Task WriteAsync
        (
            string? value
        )
    {
        Write (value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteAsync(char[],int,int)"/>
    public override Task WriteAsync
        (
            char[] buffer,
            int index,
            int count
        )
    {
        Write (buffer, index, count);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteLineAsync(char)"/>
    public override Task WriteLineAsync
        (
            char value
        )
    {
        WriteLine (value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteLineAsync(string?)"/>
    public override Task WriteLineAsync
        (
            string? value
        )
    {
        WriteLine (value);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteLineAsync(char[],int,int)"/>
    public override Task WriteLineAsync
        (
            char[] buffer,
            int index,
            int count
        )
    {
        WriteLine (buffer, index, count);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.Write(bool)"/>
    public override void Write
        (
            bool value
        )
    {
        AssertNotDisposed();
        _builder.Append (value);
    }

    /// <inheritdoc cref="TextWriter.Write(decimal)"/>
    public override void Write
        (
            decimal value
        )
    {
        AssertNotDisposed();
        _builder.Append (value);
    }

    /// <inheritdoc cref="TextWriter.FlushAsync"/>
    public override Task FlushAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Materializes the current state from underlying string builder.
    /// </summary>
    public override string ToString()
    {
        return _builder.ToString();
    }

    /// <inheritdoc cref="TextWriter.Write(System.ReadOnlySpan{char})"/>
    public override void Write
        (
            ReadOnlySpan<char> buffer
        )
    {
        AssertNotDisposed();

        _builder.Append (buffer);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(System.ReadOnlySpan{char})"/>
    public override void WriteLine
        (
            ReadOnlySpan<char> buffer
        )
    {
        AssertNotDisposed();

        _builder.Append (buffer);
        WriteLine();
    }

    /// <inheritdoc cref="TextWriter.WriteAsync(System.ReadOnlyMemory{char},System.Threading.CancellationToken)"/>
    public override Task WriteAsync
        (
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default
        )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled (cancellationToken);
        }

        Write (buffer.Span);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="TextWriter.WriteLineAsync(System.ReadOnlyMemory{char},System.Threading.CancellationToken)"/>
    public override Task WriteLineAsync
        (
            ReadOnlyMemory<char> buffer,
            CancellationToken cancellationToken = default
        )
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled (cancellationToken);
        }

        WriteLine (buffer.Span);
        return Task.CompletedTask;
    }

    private void AssertNotDisposed()
    {
        if (!_isOpen)
        {
            throw new ObjectDisposedException (nameof (_builder));
        }
    }

    #endregion
}
