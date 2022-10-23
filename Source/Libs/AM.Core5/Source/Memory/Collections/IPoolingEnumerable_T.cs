// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPoolingEnumerable_T.cs -- интерфейс типизированной перечисляемой коллекции
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

#region Using directives

using System.Collections;

#endregion

#nullable enable

namespace AM.Memory.Collections;

/// <summary>
/// Интерфейс типизированной перечисляемой коллекции.
/// </summary>
public interface IPoolingEnumerable<out T>
    : IPoolingEnumerable
{
    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    new IPoolingEnumerator<T> GetEnumerator();
}
