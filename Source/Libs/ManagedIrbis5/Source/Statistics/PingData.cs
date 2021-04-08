// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PingData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

namespace ManagedIrbis.Statistics
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("ping")]
    public struct PingData
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Moment.
        /// </summary>
        [XmlElement("moment")]
        [JsonPropertyName("moment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Success.
        /// </summary>
        [XmlElement("success")]
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Roundtrip time.
        /// </summary>
        [XmlElement("roundtrip")]
        [JsonPropertyName("roundtrip")]
        public int RoundTripTime { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Success"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSuccess()
        {
            return Success;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Moment = new DateTime(reader.ReadPackedInt64());
            Success = reader.ReadBoolean();
            RoundTripTime = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt64(Moment.Ticks);
            writer.Write(Success);
            writer.WritePackedInt32(RoundTripTime);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString() => $"{Moment:HH:mm:ss} {Success} {RoundTripTime}";

        #endregion
    }
}
