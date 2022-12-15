// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LoanMemory.cs -- память взаймы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Text;

#endregion

#nullable enable

namespace AM.Buffers;

/// <summary>
/// Память взаймы из пула для коротких операций.
/// </summary>
/// <example>
/// <code>
/// using var memory = new LoanMemory&lt;byte&gt; (6);
/// memory[0] = (byte) 'H';
/// memory[1] = (byte) 'e';
/// memory[2] = (byte) 'l';
/// memory[3] = (byte) 'l';
/// memory[4] = (byte) 'o';
/// memory[5] = (byte) '!';
///
/// networkStream.Write (memory.AsSpan());
/// </code>
/// </example>
public readonly ref struct LoanMemory<T>
{
    #region Properties

    /// <summary>
    /// Собственно взятый взаймы массив.
    /// </summary>
    public T[] Array { get; }

    /// <summary>
    /// Точная длина массива (пул может выдать более длинный массив).
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Пул, из которого был взят массив.
    /// </summary>
    public ArrayPool<T> Pool { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="length">Минимальная длина запрашиваемого массива.
    /// </param>
    /// <param name="pool">Пул, который предполагается использовать.
    /// </param>
    public LoanMemory
        (
            int length,
            ArrayPool<T>? pool = null
        )
    {
        Sure.Positive (length);

        Pool = pool ?? ArrayPool<T>.Shared;
        Array = Pool.Rent (length);
        Length = length;
    }

    private LoanMemory
        (
            T[] array,
            int length,
            ArrayPool<T> pool
        )
    {
        Array = array;
        Length = length;
        Pool = pool;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Фрагмент массива, строго соответствующий запрошенной длине.
    /// </summary>
    public Memory<T> AsMemory() => Array.AsMemory (0, Length);

    /// <summary>
    /// Фрагмент массива, строго соответствующий запрошенной длине.
    /// </summary>
    public Span<T> AsSpan() => Array.AsSpan (0, Length);

    /// <summary>
    /// Возврат массива в пул.
    /// </summary>
    public void Dispose()
    {
        Pool.Return (Array);
    }

    /// <summary>
    /// Получение взаймы массива символов для числа.
    /// </summary>
    /// <param name="number">Число для расформатирования.</param>
    /// <param name="format">Формат (опционально).</param>
    /// <param name="provider">Провайдер формата (опционально).</param>
    /// <param name="pool">Пул для использования.</param>
    /// <typeparam name="TNumber">Тип числа.</typeparam>
    /// <returns>Полученный массив.</returns>
    public static LoanMemory<char> ForNumber<TNumber>
        (
            TNumber number,
            ReadOnlySpan<char> format = default,
            IFormatProvider? provider = null,
            ArrayPool<char>? pool = null
        )
        where TNumber: ISpanFormattable
    {
        pool ??= ArrayPool<char>.Shared;
        var length = 10;
        var array = pool.Rent (length);

        int written;
        while (!number.TryFormat (array, out written, format, provider))
        {
            pool.Return (array);
            length = length * 3 / 2;
            array = pool.Rent (length);
        }

        return new LoanMemory<char> (array, written, pool);
    }

    /// <summary>
    /// Получение взаймы массива байт для указанного текста.
    /// </summary>
    public static LoanMemory<byte> ForText
        (
            ReadOnlySpan<char> text,
            Encoding? encoding = null,
            ArrayPool<byte>? pool = null
        )
    {
        encoding ??= Encoding.UTF8;
        var length = encoding.GetByteCount (text);

        var result = new LoanMemory<byte> (length, pool);
        encoding.GetBytes (text, result.Array);

        return result;
    }

    #endregion
}
