// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SpanBuilder.cs -- сверхэффективное склеивание кусков памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Сверхэффективное склеивание кусков памяти.
/// </summary>
/// <typeparam name="T">Тип данных.</typeparam>
public ref struct SpanBuilder<T>
    where T: struct
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public SpanBuilder
        (
            Span<T> memory
        )
    {
        _memory = memory;
        _position = 0;
    }

    #endregion

    #region Private members

    private Span<T> _memory;
    private int _position;

    #endregion

    #region Public methods

    /// <summary>
    /// Приклеивание очередного фрагмента памяти.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            ReadOnlySpan<T> span
        )
    {
        span.CopyTo (_memory[_position..]);
        _position += span.Length;
    }

    /// <summary>
    /// Выдача результата.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public Span<T> Build()
    {
        return _memory[.._position];
    }

    #endregion
}
