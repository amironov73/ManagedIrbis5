// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RelatedField.cs -- связанное поле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Связанное поле.
    /// </summary>
    [XmlRoot("related")]
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class RelatedField
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        [XmlElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public FieldSchema? Field { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonPropertyName("tag")]
        public int Tag { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        public static RelatedField ParseElement
            (
                XElement element
            )
        {
            var result = new RelatedField
            {
                Tag = element.GetAttributeText("tag", null).SafeToInt32(),
                Name = element.GetAttributeText("name", null),
                Description = element.GetInnerXml("DESCRIPTION")
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Description"/> field?
        /// </summary>
        public bool ShouldSerializeDescription()
        {
            return !string.IsNullOrEmpty(Description);
        }

        /// <summary>
        /// Should serialize the <see cref="Name"/> field?
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeName()
        {
            return !string.IsNullOrEmpty(Name);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Description = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Tag = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Description)
                .WriteNullable(Name)
                .WritePackedInt32(Tag);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"[{Tag}]: {Name.ToVisibleString()}";

        #endregion

    } // class RelatedField

} // namespace ManagedIrbis.Marc.Schema

