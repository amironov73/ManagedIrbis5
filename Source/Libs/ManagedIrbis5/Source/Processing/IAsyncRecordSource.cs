// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IAsyncRecordSource.cs -- интерфейс асинхронного источника записей для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Processing;

/// <summary>
/// Интерфейс асинхронного источника записей для глобальной корректировки.
/// </summary>
public interface IAsyncRecordSource
    : IAsyncDisposable
{
    /// <summary>
    /// Получение следующей записи из источника.
    /// </summary>
    /// <returns>
    /// <c>null</c> означает, что записи в источнике закончились.
    /// </returns>
    Task<Record?> GetNextRecordAsync();

    /// <summary>
    /// Получение общего количества записей,
    /// подаваемых на глобальную корректировку.
    /// </summary>
    /// <returns>
    /// Отрицательное число означает, что количество записей неизвестно.
    /// </returns>
    Task<int> GetRecordCountAsync();
}
