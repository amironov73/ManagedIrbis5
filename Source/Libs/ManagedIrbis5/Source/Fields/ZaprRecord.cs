// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ZaprRecord.cs -- запись в базе данных ZAPR
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

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
    [XmlRoot ("zapr")]
    public sealed class ZaprRecord
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Идентификатор читателя.
        /// </summary>
        [Field (1)]
        [XmlElement ("ticket")]
        [JsonPropertyName ("ticket")]
        [Description ("Идентификатор читателя")]
        [DisplayName ("Идентификатор читателя")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Постоянные запросы.
        /// </summary>
        [Field (2)]
        [XmlElement ("request")]
        [JsonPropertyName ("zapr")]
        [Description ("Постоянные запросы")]
        [DisplayName ("Постоянные запросы")]
        public ZaprInfo[]? Requests { get; set; }

        /// <summary>
        /// Связанная запись в нераскодированном виде.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Record? Record { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение данных к записи.
        /// </summary>
        public Record ApplyTo
            (
                Record record
            )
        {
            Sure.NotNull (record);

            record.RemoveField (1);
            if (!string.IsNullOrEmpty (Ticket))
            {
                record.Add (1, Ticket);
            }

            record.RemoveField (2);
            if (Requests is not null)
            {
                foreach (var request in Requests)
                {
                    record.Add (request.ToField());
                }
            }

            return record;
        }

        /// <summary>
        /// Разбор записи из базы данных.
        /// </summary>
        public static ZaprRecord ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new ZaprRecord
            {
                Ticket = record.FM (1),
                Requests = ZaprInfo.ParseRecord (record),
                Record = record
            };

            return result;
        }

        /// <summary>
        /// Преобразование данных в запись.
        /// </summary>
        public Record ToRecord()
        {
            var result = new Record();

            if (!string.IsNullOrEmpty (Ticket))
            {
                result.Add (1, Ticket);
            }

            if (Requests is not null)
            {
                foreach (var request in Requests)
                {
                    result.Add (request.ToField());
                }
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Ticket = reader.ReadNullableString();
            Requests = reader.ReadNullableArray<ZaprInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Ticket)
                .WriteNullableArray (Requests);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ZaprRecord>(this, throwOnError);

            verifier
                .NotNullNorEmpty (Ticket);

            if (Requests is not null)
            {
                foreach (var request in Requests)
                {
                    verifier.VerifySubObject (request);
                }
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Ticket.ToVisibleString();
        }

        #endregion

    } // class ZaprRecord

} // namespace ManagedIrbis.Fields
