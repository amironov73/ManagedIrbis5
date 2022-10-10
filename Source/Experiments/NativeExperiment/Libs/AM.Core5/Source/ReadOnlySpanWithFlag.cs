// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ReadOnlySpanWithFlag.cs -- спан с флагом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Спан с флагом.
/// </summary>
public readonly ref struct ReadOnlySpanWithFlag<T>
{
    #region Properties

    /// <summary>
    /// Собственно флаг.
    /// </summary>
    public readonly bool Flag;

    /// <summary>
    /// Собственно спан.
    /// </summary>
    public readonly ReadOnlySpan<T> Span;

    /// <summary>
    /// Индексатор.
    /// </summary>
    public ref readonly T this [int index] => ref Span[index];

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length => Span.Length;

    /// <summary>
    /// Спан пустой?
    /// </summary>
    public bool IsEmpty => Span.IsEmpty;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlySpanWithFlag
        (
            bool flag
        )
    {
        Flag = flag;
        Span = default;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlySpanWithFlag
        (
            bool flag,
            ReadOnlySpan<T> span
        )
    {
        Flag = flag;
        Span = span;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlySpanWithFlag
        (
            bool flag,
            T[]? array
        )
    {
        Flag = flag;
        Span = array;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlySpanWithFlag
        (
            bool flag,
            T[]? array,
            int start,
            int length
        )
    {
        Flag = flag;
        Span = new ReadOnlySpan<T> (array, start, length);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public unsafe ReadOnlySpanWithFlag
        (
            bool flag,
            void *pointer,
            int length
        )
    {
        Flag = flag;
        Span = new ReadOnlySpan<T> (pointer, length);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение закрепляемой ссылки.
    /// </summary>
    public ref readonly T GetPinnableReference() => ref Span.GetPinnableReference();

    /// <summary>
    /// Срез.
    /// </summary>
    public ReadOnlySpanWithFlag<T> Slice (int start) => new (Flag, Span.Slice (start));

    /// <summary>
    /// Срез.
    /// </summary>
    public ReadOnlySpanWithFlag<T> Slice (int start, int length) => new (Flag, Span.Slice (start, length));

    /// <summary>
    /// Преобразование в массив.
    /// </summary>
    public T[] ToArray() => Span.ToArray();

    #endregion

    #region Operators

    /// <summary>
    /// Оператор нефвного преобразования в логическое значение.
    /// </summary>
    public static implicit operator bool (ReadOnlySpanWithFlag<T> span) => span.Flag;

    /// <summary>
    /// Оператор равенства.
    /// </summary>
    public static bool operator ==
        (
            ReadOnlySpanWithFlag<T> left,
            ReadOnlySpanWithFlag<T> right
        )
        => left.Flag == right.Flag && left.Span == right.Span;

    /// <summary>
    /// Оператор неравенства.
    /// </summary>
    public static bool operator !=
        (
            ReadOnlySpanWithFlag<T> left,
            ReadOnlySpanWithFlag<T> right
        )
        => left.Flag != right.Flag || left.Span != right.Span;

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public ReadOnlySpan<T>.Enumerator GetEnumerator() => Span.GetEnumerator();

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => $"{Flag}, {Span.ToString()}";

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? obj) => throw new NotSupportedException();

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode() => throw new NotSupportedException();

    #endregion
}
