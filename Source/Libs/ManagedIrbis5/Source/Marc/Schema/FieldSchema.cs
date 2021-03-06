﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FieldSchema.cs -- схема поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
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
    /// Схема поля.
    /// </summary>
    [XmlRoot("field")]
    [DebuggerDisplay("[{Tag}] {Name}")]
    public sealed class FieldSchema
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Display.
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// Examples.
        /// </summary>
        public NonNullCollection<Example> Examples { get; } = new();

        /// <summary>
        /// First indicator.
        /// </summary>
        public IndicatorSchema Indicator1 { get; private set; } = new();

        /// <summary>
        /// Second indicator.
        /// </summary>
        public IndicatorSchema Indicator2 { get; private set; } = new();

        /// <summary>
        /// Mandatory?
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? MandatoryText { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Related fields
        /// </summary>
        public NonNullCollection<RelatedField> RelatedFields { get; } = new();

        /// <summary>
        /// Repeatable?
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? RepeatableText { get; set; }

        /// <summary>
        /// Subfields.
        /// </summary>
        public NonNullCollection<SubFieldSchema> SubFields { get; } = new();

        /// <summary>
        /// Tag.
        /// </summary>
        public string? Tag { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse given XML element.
        /// </summary>
        public static FieldSchema ParseElement
            (
                XElement element
            )
        {
            var result = new FieldSchema
            {
                Tag = element.GetAttributeText("tag"),
                Name = element.GetAttributeText("name"),
                Mandatory = element.GetAttributeBoolean("mandatory"),
                MandatoryText = element.GetAttributeText("nm", null),
                Repeatable = element.GetAttributeBoolean("repeatable"),
                RepeatableText = element.GetAttributeText("nr", null),
                Description = element.GetInnerXml("DESCRIPTION"),
                Display = element.GetAttributeBoolean("display", false)
            };

            foreach (var subElement in element.Elements("SUBFIELD"))
            {
                var subField
                    = SubFieldSchema.ParseElement(subElement);
                result.SubFields.Add(subField);
            }

            foreach (var subElement in element.Elements("RELATED"))
            {
                var relatedField
                    = RelatedField.ParseElement(subElement);
                result.RelatedFields.Add(relatedField);
            }

            var examples = element.Element("EXAMPLES");
            if (!ReferenceEquals(examples, null))
            {
                foreach (var subElement in examples.Elements("EX"))
                {
                    var example = Example.ParseElement(subElement);
                    result.Examples.Add(example);
                }
            }

            var indicator1 = element.Element("IND1");
            if (!ReferenceEquals(indicator1, null))
            {
                result.Indicator1 = IndicatorSchema.ParseElement(indicator1);
            }

            var indicator2 = element.Element("IND2");
            if (!ReferenceEquals(indicator2, null))
            {
                result.Indicator2 = IndicatorSchema.ParseElement(indicator2);
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
            Description = reader.ReadNullableString();
            Display = reader.ReadBoolean();
            reader.ReadCollection(Examples);
            Indicator1.RestoreFromStream(reader);
            Indicator2.RestoreFromStream(reader);
            Mandatory = reader.ReadBoolean();
            MandatoryText = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            reader.ReadCollection(RelatedFields);
            Repeatable = reader.ReadBoolean();
            RepeatableText = reader.ReadNullableString();
            reader.ReadCollection(SubFields);
            Tag = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Description);
            writer.Write(Display);
            writer.WriteCollection(Examples);
            Indicator1.SaveToStream(writer);
            Indicator2.SaveToStream(writer);
            writer.Write(Mandatory);
            writer.WriteNullable(MandatoryText);
            writer.WriteNullable(Name);
            writer.WriteCollection(RelatedFields);
            writer.Write(Repeatable);
            writer.WriteNullable(RepeatableText);
            writer.WriteCollection(SubFields);
            writer.WriteNullable(Tag);
        }

        #endregion

    } // class FieldSchema

} // namespace ManagedIrbis.Marc.Schema
