// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPoolingEnumerable.cs -- интерфейс перечисляемой коллекции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace AM.Memory.Collections;

/// <summary>
/// Массив из двух элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (2)]
public struct InlineArray2<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 2);
}

/// <summary>
/// Массив из трех элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (3)]
public struct InlineArray3<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 3);
}

/// <summary>
/// Массив из четырех элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (4)]
public struct InlineArray4<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 4);
}

/// <summary>
/// Массив из пяти элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (5)]
public struct InlineArray5<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 5);
}

/// <summary>
/// Массив из шести элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (6)]
public struct InlineArray6<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 6);
}

/// <summary>
/// Массив из семи элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (7)]
public struct InlineArray7<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 7);
}

/// <summary>
/// Массив из восьми элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (8)]
public struct InlineArray8<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 8);
}

/// <summary>
/// Массив из девяти элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (9)]
public struct InlineArray9<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 9);
}

/// <summary>
/// Массив из десяти элементов, размещаемый на стеке.
/// Допускает class-элементы, в отличие от stackalloc.
/// </summary>
[PublicAPI]
[InlineArray (10)]
public struct InlineArray10<T>
{
    private T _element0;

    /// <summary>
    /// Получение спана от массива.
    /// </summary>
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan (ref _element0, 10);
}
