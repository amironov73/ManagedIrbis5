// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PrinterDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Json;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Windows.Forms.Printing
{
    /// <summary>
    /// Printer definition for saving/restoring.
    /// </summary>
    [XmlRoot("printer")]
    public sealed class PrinterDefinition
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Как Windows называет принтер.
        /// </summary>
        [XmlElement("name")]
        [JsonPropertyName("name")]
        [DisplayName("Принтер")]
        public string? Name { get; set; }

        /// <summary>
        /// Ширина страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("width")]
        [JsonPropertyName("width")]
        [DisplayName("Ширина страницы")]
        public int PageWidth { get; set; }

        /// <summary>
        /// Высота страницы в сотых долях дюйма.
        /// </summary>
        [XmlElement("height")]
        [JsonPropertyName("height")]
        [DisplayName("Высота страницы")]
        public int PageHeight { get; set; }

        /// <summary>
        /// Ландшафтная ориентация страницы?
        /// </summary>
        [XmlElement("landscape")]
        [JsonPropertyName("landscape")]
        [DisplayName("Ландшафтная ориентация")]
        public bool Landscape { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Get Fargo Card printer.
        /// </summary>
        public static PrinterDefinition GetFargoPrinter()
        {
            var result = new PrinterDefinition
            {
                Name = "HDP5000 Card Printer",
                PageWidth = 220,
                PageHeight = 345,
                Landscape = true
            };

            return result;
        }

        /// <summary>
        /// Загрузка определения принтера из JSON-файла.
        /// </summary>
        public static PrinterDefinition LoadJson ( string fileName ) =>
            JsonUtility.ReadObjectFromFile<PrinterDefinition> (fileName);

        /// <summary>
        /// Загрузка определения принтера из XML-файла.
        /// </summary>
        public static PrinterDefinition LoadXml
            (
                string fileName
            )
        {
            using var stream = File.OpenRead(fileName);
            var serializer = new XmlSerializer(typeof(PrinterDefinition));

            return (PrinterDefinition)serializer.Deserialize(stream).ThrowIfNull();
        }

        /// <summary>
        /// Save the <see cref="PrinterDefinition"/>
        /// to the JSON file.
        /// </summary>
        public void SaveJson
            (
                string fileName
            )
        {
            JsonUtility.SaveObjectToFile
                (
                    this,
                    fileName
                );
        }

        /// <summary>
        /// Save the <see cref="PrinterDefinition"/>
        /// to the XML file.
        /// </summary>
        public void SaveXml
            (
                string fileName
            )
        {
            using var stream = File.Create(fileName);
            var serializer = new XmlSerializer(typeof(PrinterDefinition));
            serializer.Serialize(stream, this);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            PageWidth = reader.ReadPackedInt32();
            PageHeight = reader.ReadPackedInt32();
            Landscape = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Name)
                .WritePackedInt32(PageWidth)
                .WritePackedInt32(PageHeight)
                .Write(Landscape);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify the object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<PrinterDefinition>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .Assert(PageWidth > 0, "PageWidth")
                .Assert(PageHeight > 0, "PageHeight");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Name}: {PageWidth} x {PageHeight}";

        #endregion

    } // class PrinterDefinition

} // namespace AM.Windows.Forms.Printing
