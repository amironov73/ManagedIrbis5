// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MarcSchema.cs -- схема MARC-записи
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
    /// Схема MARC-записи.
    /// </summary>
    [XmlRoot("schema")]
    [DebuggerDisplay("Fields = {Fields.Count}")]
    public sealed class MarcSchema
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Fields.
        /// </summary>
        [JsonPropertyName("fields")]
        [XmlElement("field")]
        public NonNullCollection<FieldSchema> Fields { get; private set; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Parse given XML document.
        /// </summary>
        public static MarcSchema ParseDocument
            (
                XDocument document
            )
        {
            MarcSchema result = new MarcSchema();

            var elements = document.Descendants("FIELD");
            foreach (XElement element in elements)
            {
                FieldSchema field = FieldSchema.ParseElement(element);
                result.Fields.Add(field);
            }

            return result;
        }

        /// <summary>
        /// Parse local XML file.
        /// </summary>
        public static MarcSchema ParseLocalXml
            (
                string fileName
            )
        {
            XDocument document = XDocument.Load(fileName);
            MarcSchema result = ParseDocument(document);

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
            reader.ReadCollection(Fields);
        }

        /// <summary>
        /// Save object stat to the given stream
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            /*
            writer.WriteCollection(Fields);
            */

            throw new NotImplementedException();
        }

        #endregion

    } // class MarcSchema

} // namespace ManagedIrbis.Marc.Schema
