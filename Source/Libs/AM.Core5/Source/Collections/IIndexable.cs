// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IIndexable.cs -- интерфейс индексируемого объекта
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections;

/// <summary>
/// Indexable object interface.
/// </summary>
public interface IIndexable<T>
{
    /// <summary>
    /// Получение объекта по указанному индексу.
    /// </summary>
    T? this [int index] { get; }

    /// <summary>
    /// Общее количество проиндексированных элементов.
    /// </summary>
    int Count { get; }
}
