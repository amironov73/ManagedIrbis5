// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BuilderUtility.cs -- полезные методы для SpanBuilder
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Полезные методы для <see cref="SpanBuilder{T}"/> и <see cref="UnsafeBuilder{T}"/>.
/// </summary>
public static class BuilderUtility
{
    #region Public methods

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref SpanBuilder<char> builder,
            in T value
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, default, default))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref SpanBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format, default))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref SpanBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format,
            IFormatProvider provider
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format, provider))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref UnsafeBuilder<char> builder,
            in T value
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, default, default))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref UnsafeBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format, default))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием.
    /// </summary>
    public static unsafe void AppendFormat<T>
        (
            this ref UnsafeBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format,
            IFormatProvider provider
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format, provider))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием (инвариантная культура).
    /// </summary>
    public static unsafe void AppendInvariant<T>
        (
            this ref SpanBuilder<char> builder,
            in T value
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, default,
                CultureInfo.InvariantCulture))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием (инвариантная культура).
    /// </summary>
    public static unsafe void AppendInvariant<T>
        (
            this ref SpanBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format,
                CultureInfo.InvariantCulture))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием (инвариантная культура).
    /// </summary>
    public static unsafe void AppendInvariant<T>
        (
            this ref UnsafeBuilder<char> builder,
            in T value
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, default,
                CultureInfo.InvariantCulture))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    /// <summary>
    /// Добавление значения с форматированием (инвариантная культура).
    /// </summary>
    public static unsafe void AppendInvariant<T>
        (
            this ref UnsafeBuilder<char> builder,
            in T value,
            ReadOnlySpan<char> format
        )
        where T: ISpanFormattable
    {
        Span<char> temporary = stackalloc char[36];
        if (!value.TryFormat (temporary, out var written, format,
                CultureInfo.InvariantCulture))
        {
            throw new ArsMagnaException();
        }

        builder.Append (temporary[..written]);
    }

    #endregion
}
