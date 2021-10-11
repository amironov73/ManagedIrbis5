// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* PerfRecord.cs -- запись о произведенной сетевой транзакции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Запись о произведенной сетевой транзакции.
    /// </summary>
    [XmlRoot("transaction")]
    public sealed class PerfRecord
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Момент времени.
        /// </summary>
        [XmlAttribute ("moment")]
        [JsonPropertyName ("moment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Хост, с которым происходил обмен данными.
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// Код операции.
        /// </summary>
        [XmlAttribute ("code")]
        [JsonPropertyName ("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Размер исходящего пакета (байты).
        /// </summary>
        [XmlAttribute ("outgoing")]
        [JsonPropertyName ("outgoing")]
        public int OutgoingSize { get; set; }

        /// <summary>
        /// Размер входящего пакета (байты).
        /// </summary>
        [XmlAttribute ("incoming")]
        [JsonPropertyName ("incoming")]
        public int IncomingSize { get; set; }

        /// <summary>
        /// Затрачено времени (миллисекунды).
        /// </summary>
        [XmlAttribute ("elapsed")]
        [JsonPropertyName ("elapsed")]
        public long ElapsedTime { get; set; }

        /// <summary>
        /// Сообщение об ошибке (если есть).
        /// </summary>
        [XmlElement ("error")]
        [JsonPropertyName ("error")]
        public string? ErrorMessage { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader, nameof (reader));

            Moment = reader.ReadDateTime();
            Host = reader.ReadNullableString();
            Code = reader.ReadNullableString();
            OutgoingSize = reader.ReadPackedInt32();
            IncomingSize = reader.ReadPackedInt32();
            ElapsedTime = reader.ReadPackedInt64();
            ErrorMessage = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer, nameof (writer));

            writer
                .Write (Moment)
                .WriteNullable (Host)
                .WriteNullable (Code)
                .WritePackedInt32 (OutgoingSize)
                .WritePackedInt32 (IncomingSize)
                .WritePackedInt64 (ElapsedTime)
                .WriteNullable (ErrorMessage);

        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.Format
            (
                CultureInfo.InvariantCulture,
                "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                Moment,
                Host,
                Code,
                OutgoingSize,
                IncomingSize,
                ElapsedTime,
                ErrorMessage
            );

        #endregion

    } // class PerfRecord

} // namespace ManagedIrbis.Performance
