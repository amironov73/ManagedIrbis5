// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Example.cs -- пример
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Пример.
    /// </summary>
    [XmlRoot("example")]
    [DebuggerDisplay("{" + nameof(Text) + "}")]
    public sealed class Example
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Number.
        /// </summary>
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        public int Number { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        [XmlText]
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        public static Example ParseElement
            (
                XElement element
            )
        {
            var result = new Example
            {
                Number = element.GetAttributeInt32("N", 0),
                Text = element.GetInnerXml()
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the given stream
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Number = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Number)
                .WriteNullable(Text);
        }

        #endregion

        #region Object members

        #endregion

    } // class Example

} // namespace ManagedIrbis.Marc.Schema
