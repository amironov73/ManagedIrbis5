// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ISyncRecordSource.cs -- интерфейс синхронного источника записей для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    /// <summary>
    /// Интерфейс синхронного источника записей для глобальной корректировки.
    /// </summary>
    public interface ISyncRecordSource
        : IDisposable
    {
        /// <summary>
        /// Получение следующей записи из источника.
        /// </summary>
        /// <returns>
        /// <c>null</c> означает, что записи в источнике закончились.
        /// </returns>
        Record? GetNextRecord();

        /// <summary>
        /// Получение общего количества записей,
        /// подаваемых на глобальную корректировку.
        /// </summary>
        /// <returns>
        /// Отрицательное число означает, что количество записей неизвестно.
        /// </returns>
        int GetRecordCount();

    } // interface ISyncRecordSource

} // namespace ManagedIrbis.Processing
