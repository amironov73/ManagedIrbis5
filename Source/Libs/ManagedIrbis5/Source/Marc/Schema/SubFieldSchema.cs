// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SubFieldSchema.cs -- схема подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
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
    /// Схема подполя.
    /// </summary>
    [XmlRoot("subfield")]
    [DebuggerDisplay("[{Code}] {Name}")]
    public sealed class SubFieldSchema
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Code.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public char Code { get; set; }

        /// <summary>
        /// For serialization.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute("code")]
        [JsonPropertyName("code")]
        public string CodeString
        {
            get
            {
                return Code < ' '
                    ? " "
                    : Code.ToString();
            }
            set
            {
                Code = string.IsNullOrEmpty(value)
                    ? '\0'
                    : value[0];
            }
        }

        /// <summary>
        /// Description.
        /// </summary>
        [XmlElement("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Display.
        /// </summary>
        [XmlAttribute("display")]
        [JsonPropertyName("display")]
        public bool Display { get; set; }

        /// <summary>
        /// Mandatory?
        /// </summary>
        [XmlAttribute("mandatory")]
        [JsonPropertyName("mandatory")]
        public bool Mandatory { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlElement("mandatory-text")]
        [JsonPropertyName("mandatory-text")]
        public string MandatoryText { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Repeatable?
        /// </summary>
        [XmlAttribute("repeatable")]
        [JsonPropertyName("repeatable")]
        public bool Repeatable { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlElement("repeatable-text")]
        [JsonPropertyName("repeatable-text")]
        public string? RepeatableText { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        public static SubFieldSchema ParseElement
            (
                XElement element
            )
        {
            var result = new SubFieldSchema
            {
                Code = element.GetAttributeCharacter("tag"),
                Name = element.GetAttributeText("name", null),
                Mandatory = element.GetAttributeBoolean("mandatory", false),
                MandatoryText = element.GetAttributeText("nm", null),
                Repeatable = element.GetAttributeBoolean("repeatable", false),
                RepeatableText = element.GetAttributeText("nr", null),
                Description = element.GetInnerXml("DESCRIPTION"),
                Display = element.GetAttributeBoolean("display", false)
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
        /// Should serialize the <see cref="MandatoryText"/> field?
        /// </summary>
        public bool ShouldSerializeMandatoryText()
        {
            return !string.IsNullOrEmpty(MandatoryText);
        }

        /// <summary>
        /// Should serialize the <see cref="Name"/> field?
        /// </summary>
        public bool ShouldSerializeName()
        {
            return !string.IsNullOrEmpty(Name);
        }

        /// <summary>
        /// Should serialize the <see cref="RepeatableText"/> field?
        /// </summary>
        public bool ShouldSerializeRepeatableText()
        {
            return !string.IsNullOrEmpty(RepeatableText);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadChar();
            Description = reader.ReadNullableString();
            Display = reader.ReadBoolean();
            Mandatory = reader.ReadBoolean();
            MandatoryText = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Repeatable = reader.ReadBoolean();
            RepeatableText = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Code);
            writer.WriteNullable(Description);
            writer.Write(Display);
            writer.Write(Mandatory);
            writer.WriteNullable(MandatoryText);
            writer.WriteNullable(Name);
            writer.Write(Repeatable);
            writer.WriteNullable(RepeatableText);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"'{CodeString.ToVisibleString()}' : {Name.ToVisibleString()}";

        #endregion

    } // class SubFieldSchema

} // namespace ManagedIrbis.Marc.Schema
