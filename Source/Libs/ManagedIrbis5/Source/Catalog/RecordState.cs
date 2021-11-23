// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordState.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog
{
    /// <summary>
    /// State of the <see cref="Record"/>.
    /// </summary>
    [XmlRoot ("record")]
    [DebuggerDisplay ("{Mfn} {Status} {Version}")]
    public struct RecordState
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute ("mfn")]
        [JsonPropertyName ("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        [XmlAttribute ("status")]
        [JsonPropertyName ("status")]
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        [XmlAttribute ("version")]
        [JsonPropertyName ("version")]
        public int Version { get; set; }

        #endregion

        #region Private members

        private static char[] _delimiters =
        {
            ' ', '\t', '\r', '\n', '#', '\x1F', '\x1E'
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server answer.
        /// </summary>
        public static RecordState ParseServerAnswer
            (
                string line
            )
        {
            //
            // &uf('G0$',&uf('+0'))
            //
            // 0 MFN#STATUS 0#VERSION OTHER
            // 0 161608#0 0#1 101#
            //

            var result = new RecordState();

            var parts = line.Split
                (
                    _delimiters,
                    StringSplitOptions.RemoveEmptyEntries
                );

            if (parts.Length < 5)
            {
                Magna.Error
                    (
                        "RecordState::ParseServerAnswer: "
                        + "bad line format: "
                        + line
                    );

                throw new IrbisException ("bad line format");
            }

            result.Mfn = parts[1].ParseInt32();
            result.Status = (RecordStatus)parts[2].ParseInt32();
            result.Version = parts[4].ParseInt32();

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Status"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeStatus()
        {
            return Status != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Version"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeVersion()
        {
            return Version != 0;
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

            Id = reader.ReadPackedInt32();
            Mfn = reader.ReadPackedInt32();
            Status = (RecordStatus)reader.ReadPackedInt32();
            Version = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WritePackedInt32 (Id)
                .WritePackedInt32 (Mfn)
                .WritePackedInt32 ((int)Status)
                .WritePackedInt32 (Version);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return $"{Mfn}:{(int)Status}:{Version}";
        }

        #endregion
    }
}
