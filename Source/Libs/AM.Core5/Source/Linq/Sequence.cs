// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Sequence.cs -- возня вокруг IEnumerable
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

#nullable enable

namespace AM.Linq;

/// <summary>
/// Возня вокруг <see cref="IEnumerable{T}"/>.
/// </summary>
public static class Sequence
{
    #region Public methods

    /// <summary>
    /// Получение всей последовательности IDisposable-объектов
    /// в виде массива. Если при получении любого объекта происходит
    /// ошибка, все они освобождаются, а исключение пробрасывается
    /// в пользовательский код.
    /// </summary>
    public static T[] Acquire<T>
        (
            this IEnumerable<T> list
        )
        where T : IDisposable
    {
        var result = new List<T>();
        try
        {
            result.AddRange (list);
        }
        catch
        {
            foreach (var disposable in result)
            {
                disposable.Dispose();
            }

            throw;
        }

        return result.ToArray();
    }

    /// <summary>
    /// Добавление двух элементов в конец последовательности.
    /// </summary>
    public static IEnumerable<T?> Append<T>
        (
            this IEnumerable<T?> head,
            Func<T?> tail
        )
    {
        foreach (var item in head)
        {
            yield return item;
        }

        yield return tail();
    }

    /// <summary>
    /// Добавление двух элементов в конец последовательности.
    /// </summary>
    /// <remarks>
    /// Метод для добавления одного элемента входит в .NET 5.
    /// </remarks>
    public static IEnumerable<T?> Append<T>
        (
            this IEnumerable<T?> head,
            T? tail1,
            T? tail2
        )
    {
        foreach (var item in head)
        {
            yield return item;
        }

        yield return tail1;
        yield return tail2;
    }

    /// <summary>
    /// Добавление двух элементов в конец последовательности.
    /// </summary>
    public static IEnumerable<T?> Append<T>
        (
            this IEnumerable<T?> head,
            Func<T?> tail1,
            Func<T?> tail2
        )
    {
        foreach (var item in head)
        {
            yield return item;
        }

        yield return tail1();
        yield return tail2();
    }

    /// <summary>
    /// Добавление трех элементов в конец последовательности.
    /// </summary>
    /// <remarks>
    /// Метод для добавления одного элемента входит в .NET 5.
    /// </remarks>
    public static IEnumerable<T?> Append<T>
        (
            this IEnumerable<T?> head,
            T? tail1,
            T? tail2,
            T? tail3
        )
    {
        foreach (var item in head)
        {
            yield return item;
        }

        yield return tail1;
        yield return tail2;
        yield return tail3;
    }

    /// <summary>
    /// Добавление трех элементов в конец последовательности.
    /// </summary>
    public static IEnumerable<T?> Append<T>
        (
            this IEnumerable<T?> head,
            Func<T?> tail1,
            Func<T?> tail2,
            Func<T?> tail3
        )
    {
        foreach (var item in head)
        {
            yield return item;
        }

        yield return tail1();
        yield return tail2();
        yield return tail3();
    }

    /// <summary>
    /// Проверка условия для всех элементов последовательности.
    /// Если условие не выполняется, генерируется исключение.
    /// </summary>
    public static IEnumerable<T?> Assert<T>
        (
            this IEnumerable<T?> list,
            Func<T?, bool> predicate,
            Func<T?, Exception>? errorSelector = null
        )
    {
        foreach (var item in list)
        {
            if (!predicate (item))
            {
                errorSelector ??= _ => throw new InvalidOperationException();
                throw errorSelector (item);
            }

            yield return item;
        }
    }

    // /// <summary>
    // /// Нарезает последовательность на куски (массивы)
    // /// не больше указанного размера.
    // /// </summary>
    // public static IEnumerable<T[]> Chunk<T>
    // (
    //     this IEnumerable<T> sequence,
    //     int pieceSize
    // )
    // {
    //     if (pieceSize <= 0)
    //     {
    //         Magna.Error
    //         (
    //             nameof(Sequence) + "::" + nameof(Chunk)
    //             + "pieceSize="
    //             + pieceSize
    //         );
    //
    //         throw new ArgumentOutOfRangeException(nameof(pieceSize));
    //     }
    //
    //     var piece = new List<T>(pieceSize);
    //     foreach (T item in sequence)
    //     {
    //         piece.Add(item);
    //         if (piece.Count >= pieceSize)
    //         {
    //             yield return piece.ToArray();
    //             piece = new List<T>(pieceSize);
    //         }
    //     }
    //
    //     if (piece.Count != 0)
    //     {
    //         yield return piece.ToArray();
    //     }
    // }

    /// <summary>
    /// Просто перебирает все элементы последовательности,
    /// чтобы "прокрутить" ее до конца.
    /// </summary>
    /// <remarks>
    /// Заимствовано из MoreLinq.
    /// </remarks>
    public static void Consume<T>
        (
            this IEnumerable<T> sequence
        )
    {
        foreach (var _ in sequence)
        {
            // Do nothing
        }
    }

    /// <summary>
    /// Вычисление последовательности функций.
    /// </summary>
    public static IEnumerable<T?> Evaluate<T>
        (
            this IEnumerable<Func<T?>> sequence
        )
    {
        foreach (var function in sequence)
        {
            yield return function();
        }
    }

    /// <summary>
    /// Первый элемент из последовательности либо значение по умолчанию.
    /// </summary>
    public static T? FirstOr<T>
        (
            this IEnumerable<T?> list,
            T? defaultValue
        )
    {

        foreach (var item in list)
        {
            return item;
        }

        return defaultValue;
    }

    /// <summary>
    /// Первый элемент из последовательности либо значение по умолчанию.
    /// </summary>
    public static T? FirstOr<T>
        (
            this IEnumerable<T?> list,
            Func<T?> defaultValue
        )
    {
        foreach (var item in list)
        {
            return item;
        }

        return defaultValue();
    }

    /// <summary>
    /// Порождает последовательность из одного элемента.
    /// </summary>
    [Pure]
    public static IEnumerable<T> FromItem<T>
        (
            T item
        )
    {
        yield return item;
    }

    /// <summary>
    /// Порождает последовательность из двух элементов.
    /// </summary>
    [Pure]
    public static IEnumerable<T> FromItems<T>
        (
            T item1,
            T item2
        )
    {
        yield return item1;
        yield return item2;
    }

    /// <summary>
    /// Порождает последовательность из трех элементов.
    /// </summary>
    [Pure]
    public static IEnumerable<T> FromItems<T>
        (
            T item1,
            T item2,
            T item3
        )
    {
        yield return item1;
        yield return item2;
        yield return item3;
    }

    /// <summary>
    /// Порождает последовательность из перечисленных элементов.
    /// </summary>
    [Pure]
    public static IEnumerable<T?> FromItems<T>
        (
            params T?[] items
        )
    {
        foreach (T? item in items)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Вычисление максимального значения в последовательности.
    /// </summary>
    public static T? MaxOrDefault<T>
        (
            this IEnumerable<T> sequence,
            T? defaultValue
        )
        where T : class
    {
        // TODO: сделать без массива

        var array = sequence.ToArray();
        if (array.Length == 0)
        {
            return defaultValue;
        }

        var result = array.Max();

        return result;
    }

    /// <summary>
    /// Вычисление максимального значения в последовательности.
    /// </summary>
    public static T? MaxOrDefault<T>
        (
            this IEnumerable<T> sequence,
            T? defaultValue
        )
        where T : struct
    {
        // TODO: сделать без массива

        var array = sequence.ToArray();
        if (array.Length == 0)
        {
            return defaultValue;
        }

        var result = array.Max();

        return result;
    }

    /// <summary>
    /// Вычисление максимального значения в последовательности.
    /// </summary>
    public static TOutput? MaxOrDefault<TInput, TOutput>
        (
            this IEnumerable<TInput> sequence,
            Func<TInput, TOutput?> selector,
            TOutput? defaultValue
        )
        where TOutput : class
    {
        var array = sequence.ToArray();
        if (array.Length == 0)
        {
            return defaultValue;
        }

        var result = array.Max (selector);

        return result;
    }

    /// <summary>
    /// Вычисление максимального значения в последовательности.
    /// </summary>
    public static TOutput? MaxOrDefault<TInput, TOutput>
        (
            this IEnumerable<TInput> sequence,
            Func<TInput, TOutput?> selector,
            TOutput? defaultValue
        )
        where TOutput : struct
    {
        var array = sequence.ToArray();
        if (array.Length == 0)
        {
            return defaultValue;
        }

        var result = array.Max (selector);

        return result;
    }

    /// <summary>
    /// Получение следующего элемента из последовательности.
    /// </summary>
    public static T? NextOrDefault<T>
        (
        this IEnumerator<T> sequence,
        T? defaultValue = default
        )
    {
        return sequence.MoveNext() ? sequence.Current : defaultValue;
    }

    /// <summary>
    /// Получение следующего элемента из последовательности.
    /// </summary>
    public static T? NextOrDefault<T>
        (
            this IEnumerator<T> sequence,
            Func<T?> defaultValue
        )
    {
        return sequence.MoveNext() ? sequence.Current : defaultValue();
    }

    // /// <summary>
    // /// Получение следующего элемента из последовательности.
    // /// </summary>
    // public static T? NextOrDefault<T> (this IEnumerator sequence, T? defaultValue = default) =>
    //     sequence.MoveNext() ? (T) sequence.Current : defaultValue;

    /// <summary>
    /// Отбирает из последовательности только
    /// ненулевые элементы.
    /// </summary>
    public static IEnumerable<T> NonNullItems<T>
        (
            this IEnumerable<T?> sequence
        )
        where T : class
    {
        foreach (var item in sequence)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Отбирает из последовательности только непустые строки.
    /// </summary>
    public static IEnumerable<string> NonEmptyLines
        (
            this IEnumerable<string?> sequence
        )
    {
        foreach (var line in sequence)
        {
            if (!string.IsNullOrEmpty (line))
            {
                yield return line;
            }
        }
    }

    /// <summary>
    /// Отбирает из последовательности только непустые строки.
    /// </summary>
    public static IEnumerable<ReadOnlyMemory<char>> NonEmptyLines
        (
            this IEnumerable<ReadOnlyMemory<char>> sequence
        )
    {
        foreach (var line in sequence)
        {
            if (!line.IsEmpty)
            {
                yield return line;
            }
        }
    }

    /// <summary>
    /// Повторяет указанное значение.
    /// </summary>
    [Pure]
    public static IEnumerable<T> Repeat<T>
        (
            T value,
            int count
        )
    {
        while (count-- > 0)
        {
            yield return value;
        }
    }

    /// <summary>
    /// Repeats the specified list.
    /// </summary>
    public static IEnumerable<T> Repeat<T>
        (
            IEnumerable<T> list,
            int count
        )
    {
        var array = list.ToArray();

        while (count-- > 0)
        {
            foreach (var value in array)
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Заменяет в последовательности одно значение на другое.
    /// </summary>
    public static IEnumerable<T> Replace<T>
        (
            this IEnumerable<T> list,
            T replaceFrom,
            T replaceTo
        )
        where T : IEquatable<T>
    {
        foreach (T item in list)
        {
            if (item.Equals (replaceFrom))
            {
                yield return replaceTo;
            }
            else
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Добавляет некоторое действие к каждому
    /// элементу последовательности.
    /// </summary>
    public static IEnumerable<T> Tee<T>
        (
            this IEnumerable<T> list,
            Action<T> action
        )
    {
        foreach (var item in list)
        {
            action (item);

            yield return item;
        }
    }

    /// <summary>
    /// Добавляет некоторое действие к каждому
    /// элементу последовательности.
    /// </summary>
    public static IEnumerable<T> Tee<T>
        (
            this IEnumerable<T> list,
            Action<int, T> action
        )
    {
        var index = 0;
        foreach (var item in list)
        {
            action (index, item);
            index++;

            yield return item;
        }
    }

    /// <summary>
    /// Перемежаем значения некоторым разделителем.
    /// </summary>
    public static IEnumerable<T> Separate<T>
        (
            this IEnumerable<T> sequence,
            T separator
        )
    {
        var first = true;
        foreach (var obj in sequence)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                yield return separator;
            }

            yield return obj;
        }
    }

    /// <summary>
    /// Попытка получить количество элементов в последовательности
    /// без перебора этой последовательности.
    /// </summary>
    public static bool TryGetCount<TSource>
        (
            this IEnumerable<TSource> source,
            out int count
        )
    {
        if (source is ICollection<TSource> collectionoft)
        {
            count = collectionoft.Count;
            return true;
        }

        if (source is IReadOnlyCollection<TSource> readOnlyCollection)
        {
            count = readOnlyCollection.Count;
            return true;
        }

        if (source is ICollection collection)
        {
            count = collection.Count;
            return true;
        }

        count = 0;

        return false;
    }

    #endregion
}
