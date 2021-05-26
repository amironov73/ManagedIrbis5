// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ZaprRecord.cs -- запись в базе данных ZAPR
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    //
    // Структура БД постоянных запросов читателей
    // Имя БД: ZAPR
    // БД ведется АВТОМАТИЧЕСКИ и не нуждается в ручной корректировке!
    // БД содержит постоянные запросы читателей.
    // Одна запись БД содержит описание постоянных запросов КОНКРЕТНОГО читателя.
    // Структура записи включает в себя следующие элементы данных (поля):
    //
    // Идентификатор читателя
    // Метка поля 1
    // Поле обязательное, неповторяющееся
    //
    // Описание постоянного запроса
    // Метка поля 2
    // Поле необязательное, повторяющееся
    // Состоит из следующих подполей:
    // A - запрос на естественном языке (поименованный читателем),
    // подполе обязательное
    // B - полнотекстовая часть запроса, подполе необязательное
    // C - библиографическая часть запроса на языке ИРБИС, подполе необязательное
    //

    /// <summary>
    /// Запись в базе данных ZAPR.
    /// </summary>
    public sealed class ZaprRecord
    {
        #region Properties

        /// <summary>
        /// Идентификатор читателя.
        /// </summary>
        [Field(1)]
        public string? Ticket { get; set; }

        /// <summary>
        /// Постоянные запросы.
        /// </summary>
        [Field(2)]
        public ZaprInfo[]? Requests { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static ZaprRecord Parse
            (
                Record record
            )
        {
            Sure.NotNull(record, nameof(record));

            var result = new ZaprRecord
            {
                Ticket = record.FM(1),
                // Requests = record.Fields
                //     .GetField(2)
                //     .Select(f => ZaprInfo.Parse(f))
                //     .ToArray()
            };

            return result;
        } // method Parse

        #endregion

    } // class ZaprRecord

} // namespace ManagedIrbis.Fields
