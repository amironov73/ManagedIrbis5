// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TransactionRecord.cs -- вся собранная прокси информация о транзакции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;

#endregion

#nullable enable

namespace ManagedIrbis.Proxying;

/// <summary>
/// Вся собранная прокси информация о транзакции "клиент-сервер".
/// </summary>
public sealed class TransactionRecord
{
    #region Properties

    /// <summary>
    /// Порядковый номер (отсчет с 1).
    /// </summary>
    public long Index { get; set; }

    /// <summary>
    /// Стадия, до которой дошло выполнение команды.
    /// Расшифровку см. <see cref="TransactionStage"/>.
    /// </summary>
    public int Stage { get; set; }

    /// <summary>
    /// Момент поступления команды (по часам прокси).
    /// </summary>
    public DateTime Moment { get; set; }

    /// <summary>
    /// Адрес, с которого пришла команда.
    /// </summary>
    public EndPoint? EndPoint { get; set; }

    /// <summary>
    /// Запрос клиента в необработанном виде
    /// (включая строки заголовка)
    /// </summary>
    public byte[]? Request { get; set; }

    /// <summary>
    /// Ответ сервера в необработанном виде
    /// (включая строки заголовка)
    /// </summary>
    public byte[]? Response { get; set; }

    /// <summary>
    /// Общая продолжительность обработки запроса
    /// со всеми пересылками туда-сюда
    /// (в миллисекундах).
    /// </summary>
    public int RoundtripDuration { get; set; }

    /// <summary>
    /// Продолжительность обработки запроса сервером
    /// ИРБИС (в миллисекундах).
    /// </summary>
    public int ServerDuration { get; set; }

    #endregion
}
