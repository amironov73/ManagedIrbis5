// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IndicatorSchema.cs -- схема индикатора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Схема индикатора.
    /// </summary>
    [XmlRoot("indicator")]
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public sealed class IndicatorSchema
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
        /// Name.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Options.
        /// </summary>
        [XmlArray("options")]
        [XmlElement("option")]
        public NonNullCollection<Option> Options { get; private set; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Parse XML element.
        /// </summary>
        public static IndicatorSchema ParseElement
            (
                XElement element
            )
        {
            IndicatorSchema result = new IndicatorSchema
            {
                Name = element.GetAttributeText("name", null),
                Description = element.GetElementText("DESCRIPTION", null)
            };

            foreach (XElement subElement in element.Elements("OPTION"))
            {
                Option option = Option.ParseElement(subElement);
                result.Options.Add(option);
            }

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
            Name = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            reader.ReadCollection(Options);
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
                .WriteNullable(Name)
                .WriteNullable(Description)
                .WriteCollection(Options);
        }

        #endregion

    } // class IndicatorSchema

} // namespace ManagedIrbis.Marc.Schema
