// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* KeyDefinition.cs -- ключ сортировки
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

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// Ключ сортировки.
    /// </summary>
    [XmlRoot ("key")]
    public sealed class KeyDefinition
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Длина ключа.
        /// </summary>
        [XmlAttribute ("length")]
        [JsonPropertyName ("length")]
        [Description ("Длина ключа")]
        public int Length { get; set; }

        /// <summary>
        /// Допустимы множественные значения?
        /// </summary>
        [XmlAttribute ("multiple")]
        [JsonPropertyName ("multiple")]
        [Description ("Допустимы множественные значения?")]
        public bool Multiple { get; set; }

        /// <summary>
        /// Спецификация формата.
        /// </summary>
        [XmlElement ("format")]
        [JsonPropertyName ("format")]
        [Description ("Спецификация ключа")]
        public string? Format { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Length = reader.ReadPackedInt32();
            Multiple = reader.ReadBoolean();
            Format = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WritePackedInt32 (Length)
                .Write (Multiple);
            writer
                .WriteNullable (Format);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<KeyDefinition> (this, throwOnError);

            // TODO implement

            verifier
                .Positive (Length)
                .NotNullNorEmpty (Format);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Format.ToVisibleString();
        }

        #endregion
    }
}
