// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPoolingGrouping.cs -- интерфейс группировки элементов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections;

/// <summary>
/// Интерфейс группировки элементов.
/// </summary>
/// <typeparam name="TKey">Тип ключа.</typeparam>
/// <typeparam name="TElement">Тип группируемых элементов.</typeparam>
public interface IPoolingGrouping<out TKey, out TElement>
    : IPoolingEnumerable<TElement>
{
    /// <summary>
    /// Ключ -- общий для всей группы элемент.
    /// </summary>
    TKey Key { get; }
}
