// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IAsyncList.cs -- асинхронный список
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Асинхронный список.
/// </summary>
public interface IAsyncList<TItem>
{
    /// <summary>
    /// Получение количества элементов в списке,
    /// </summary>
    Task<int> CountAsync { get; }

    /// <summary>
    /// Доступ к элементам списка по индексу.
    /// </summary>
    Task<TItem> this[int index] { get; }
}
