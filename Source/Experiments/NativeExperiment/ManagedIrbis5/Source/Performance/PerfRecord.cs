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
using System.ComponentModel;
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
    [XmlRoot ("transaction")]
    public sealed class PerfRecord
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Момент времени.
        /// </summary>
        [XmlAttribute ("moment")]
        [JsonPropertyName ("moment")]
        [Description ("Момент времени")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Хост, с которым происходил обмен данными.
        /// </summary>
        [XmlAttribute ("host")]
        [JsonPropertyName ("host")]
        [Description ("Хост")]
        public string? Host { get; set; }

        /// <summary>
        /// Код операции.
        /// </summary>
        [XmlAttribute ("code")]
        [JsonPropertyName ("code")]
        [Description ("Код операции")]
        public string? Code { get; set; }

        /// <summary>
        /// Размер исходящего пакета (байты).
        /// </summary>
        [XmlAttribute ("outgoing")]
        [JsonPropertyName ("outgoing")]
        [Description ("Размер исходящего пакета")]
        public int OutgoingSize { get; set; }

        /// <summary>
        /// Размер входящего пакета (байты).
        /// </summary>
        [XmlAttribute ("incoming")]
        [JsonPropertyName ("incoming")]
        [Description ("Размер входящего пакета")]
        public int IncomingSize { get; set; }

        /// <summary>
        /// Затрачено времени (миллисекунды).
        /// </summary>
        [XmlAttribute ("elapsed")]
        [JsonPropertyName ("elapsed")]
        [Description ("Затрачено времени")]
        public int ElapsedTime { get; set; }

        /// <summary>
        /// Сообщение об ошибке (если есть).
        /// </summary>
        [XmlElement ("error")]
        [JsonPropertyName ("error")]
        [Description ("Сообщение об ошибке")]
        public string? ErrorMessage { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Moment = reader.ReadDateTime();
            Host = reader.ReadNullableString();
            Code = reader.ReadNullableString();
            OutgoingSize = reader.ReadPackedInt32();
            IncomingSize = reader.ReadPackedInt32();
            ElapsedTime = reader.ReadPackedInt32();
            ErrorMessage = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .Write (Moment)
                .WriteNullable (Host)
                .WriteNullable (Code)
                .WritePackedInt32 (OutgoingSize)
                .WritePackedInt32 (IncomingSize)
                .WritePackedInt32 (ElapsedTime)
                .WriteNullable (ErrorMessage);
        }

        #endregion


        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<PerfRecord> (this, throwOnError);

            verifier
                .Assert (Moment != default);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
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
        }

        #endregion
    }
}
