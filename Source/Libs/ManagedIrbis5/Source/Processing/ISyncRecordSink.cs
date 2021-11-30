// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ISyncRecordSink.cs -- интерфейс синхронного приемника записей для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    /// <summary>
    /// Интерфейс синхронного приемника записей для глобальной корректировки.
    /// </summary>
    public interface ISyncRecordSink
        : IDisposable
    {
        /// <summary>
        /// Помещение в приемник модифицированной записи.
        /// Повторные попытки помещения одной и той же записи игнорируются.
        /// Сообщения, связанные с одной записью, склеиваются.
        /// </summary>
        void PostRecord
            (
                Record record,
                string? message = null
            );

        /// <summary>
        /// Сигнал окончания обработки записей.
        /// Может использоваться для пакетной отправки
        /// измененных записей на сервер.
        /// </summary>
        void Complete();

        /// <summary>
        /// Получение протокола.
        /// </summary>
        ProtocolLine[] GetProtocol();
    }
}
