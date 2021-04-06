// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataColumnInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("column")]
    public sealed class DataColumnInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("name")]
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the column title.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("title")]
        [XmlAttribute("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// </summary>
        /// <value>The width of the column.</value>
        [DefaultValue(0)]
        [XmlAttribute("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the data grid column.
        /// </summary>
        /// <value>The data grid column.</value>
        [DefaultValue(null)]
        [JsonPropertyName("type")]
        [XmlAttribute("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        [DefaultValue(null)]
        [XmlAttribute("defaultValue")]
        [JsonPropertyName("defaultValue")]
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// the column is frozen.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("frozen")]
        [JsonPropertyName("frozen")]
        public bool Frozen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is invisible.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("invisible")]
        [JsonPropertyName("invisible")]
        public bool Invisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the column is read only.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("readOnly")]
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// this column is sorted.
        /// </summary>
        [DefaultValue(false)]
        [XmlAttribute("sorted")]
        [JsonPropertyName("sorted")]
        public bool Sorted { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Width = reader.ReadPackedInt32();
            Type = reader.ReadNullableString();
            DefaultValue = reader.ReadNullableString();
            Frozen = reader.ReadBoolean();
            Invisible = reader.ReadBoolean();
            ReadOnly = reader.ReadBoolean();
            Sorted = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Name)
                .WriteNullable(Title)
                .WritePackedInt32(Width)
                .WriteNullable(Type)
                .WriteNullable(DefaultValue);
            writer.Write(Frozen);
            writer.Write(Invisible);
            writer.Write(ReadOnly);
            writer.Write(Sorted);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<DataColumnInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Name.ToVisibleString();

        #endregion
    }
}
