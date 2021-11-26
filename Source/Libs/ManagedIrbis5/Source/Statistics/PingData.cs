// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PingData.cs -- данные об успешности выполнения пинга до ИРБИС-сервера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

namespace ManagedIrbis.Statistics
{
    /// <summary>
    /// Данные об успешности выполнения пинга до ИРБИС-сервера.
    /// </summary>
    [XmlRoot ("ping")]
    public struct PingData
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Момент времени, когда выполнялся пинг.
        /// </summary>
        [XmlAttribute ("moment")]
        [JsonPropertyName ("moment")]
        [Description ("Момент времени")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Признак успешного выполнения.
        /// </summary>
        [XmlAttribute ("success")]
        [JsonPropertyName ("success")]
        [Description ("Признак успешного выполнения")]
        public bool Success { get; set; }

        /// <summary>
        /// Полное время обращения пакета, миллисекунды.
        /// </summary>
        [XmlAttribute ("roundtrip")]
        [JsonPropertyName ("roundtrip")]
        [Description ("Полное время обращения пакета")]
        public int RoundTripTime { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Moment = new DateTime (reader.ReadPackedInt64());
            Success = reader.ReadBoolean();
            RoundTripTime = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.WritePackedInt64 (Moment.Ticks);
            writer.Write (Success);
            writer.WritePackedInt32 (RoundTripTime);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<PingData> (this, throwOnError);

            verifier
                .Assert (Moment != default)
                .Assert (!Success || RoundTripTime > 0);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return $"{Moment:HH:mm:ss} {Success} {RoundTripTime}";
        }

        #endregion
    }
}
