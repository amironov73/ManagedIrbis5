// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPoolingEnumerable.cs -- интерфейс перечисляемой коллекции
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Memory.Collections;

/// <summary>
/// Интерфейс перечисляемой коллекции.
/// </summary>
public interface IPoolingEnumerable
{
    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IPoolingEnumerator GetEnumerator();
}
