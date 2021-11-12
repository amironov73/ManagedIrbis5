// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordInputSpecification.cs -- спецификация ввода библиографической записи
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

#nullable enable

namespace ManagedIrbis.Input
{
    /// <summary>
    /// Спецификация ввода библиографической записи.
    /// </summary>
    [XmlRoot ("record")]
    public sealed class InputSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Спецификация ввода отдельных полей.
        /// </summary>
        [XmlElement ("fields")]
        [JsonPropertyName ("fields")]
        [Description ("Спецификация ввода отдельных полей")]
        [DisplayName ("Спецификация ввода отдельных полей")]
        public FieldInputSpecification[] Fields { get; set; } = Array.Empty<FieldInputSpecification>();

        /// <summary>
        /// Имя.
        /// </summary>
        [XmlAttribute ("name")]
        [JsonPropertyName ("name")]
        [Description ("Имя")]
        [DisplayName ("Имя")]
        public string? Name { get; set; }

        /// <summary>
        /// Человеко-читаемое описание.
        /// </summary>
        [XmlAttribute ("description")]
        [JsonPropertyName ("description")]
        [Description ("Описание")]
        [DisplayName ("Описание")]
        public string? Description { get; set; }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Name = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            reader.RestoreArray<FieldInputSpecification>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable(Name)
                .WriteNullable(Description);
            Fields.SaveToStream(writer);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<InputSpecification>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty (Name)
                .NotNullNorEmpty (Description)
                .NotNull (Fields)
                .Assert (Fields.Length != 0);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Name.ToVisibleString()}: {Description.ToVisibleString()}";

        #endregion

    } // class RecordInputSpecification

} // namespace ManagedIrbis.Input

