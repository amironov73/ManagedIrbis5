// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ISyncRecordProcessor.cs -- интерфейс синхронного процессора записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Gbl;

#endregion

#nullable enable

namespace ManagedIrbis.Processing;

/// <summary>
/// Интерфейс синхронного процессора записей.
/// </summary>
public interface ISyncRecordProcessor
    : IDisposable
{
    /// <summary>
    /// Обработка одной записи.
    /// </summary>
    ProtocolLine ProcessOneRecord (Record record);

    /// <summary>
    /// Обработка множества записей.
    /// </summary>
    GblResult ProcessRecords
        (
            ISyncRecordSource source,
            ISyncRecordSink sync
        );
}
