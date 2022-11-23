// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* UnsafeBuilder.cs -- сверхэффективное склеивание кусков памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Сверхэффективное склеивание кусков памяти.
/// </summary>
public unsafe ref struct UnsafeBuilder<T>
    where T: unmanaged
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public UnsafeBuilder
        (
            T* memory,
            uint length
        )
    {
        _memory = memory;
        _length = length;
        _position = 0;
    }

    #endregion

    #region Private members

    private readonly T* _memory;
    private readonly uint _length;
    private uint _position;

    #endregion

    #region Public methods

    /// <summary>
    /// Приклеивание очередного фрагмента памяти.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            T* block,
            uint count
        )
    {
        if (_position + count > _length)
        {
            throw new InternalBufferOverflowException();
        }

        var byteCount = unchecked ((uint) (sizeof (T) * count));
        Unsafe.CopyBlock (_memory + _position, block, byteCount);
        _position += count;
    }

    /// <summary>
    /// Приклеивание очередного фрагмента памяти.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            ReadOnlySpan<T> source
        )
    {
        var position = unchecked ((int) _position);
        var length = unchecked ((int) _length);
        var target = new Span<T> (_memory, length) [position..];
        source.CopyTo (target);
        _position += unchecked ((uint) source.Length);
    }

    /// <summary>
    /// Выдача результата.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public Span<T> Build()
    {
        var position = unchecked ((int) _position);
        var length = unchecked ((int) _length);
        return new Span<T> (_memory, length)[..position];
    }

    #endregion
}
