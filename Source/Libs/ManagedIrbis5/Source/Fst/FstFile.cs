// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression

/* FstFile.cs -- файл с FST-скриптом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// FST file handling
    /// </summary>
    [XmlRoot("fst")]
    [DebuggerDisplay("FileName = {FileName}")]
    public sealed class FstFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// File name (for identification only).
        /// </summary>
        [XmlAttribute("fileName")]
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        /// <summary>
        /// Lines of the file.
        /// </summary>
        [XmlElement("line")]
        [JsonPropertyName("lines")]
        public NonNullCollection<FstLine> Lines { get; private set; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Build concatenated format string.
        /// </summary>
        public string ConcatenateFormat()
        {
            var result = new StringBuilder();

            foreach (var line in Lines)
            {
                result.Append(line.ToFormat());
            }

            return result.ToString();
        } // method ConcatenateFormat

        /// <summary>
        /// Parse the stream.
        /// </summary>
        public static FstFile ParseStream
            (
                TextReader reader
            )
        {
            var result = new FstFile();
            var lineNumber = 1;
            FstLine? line;
            while ((line = FstLine.ParseStream(reader)) != null)
            {
                line.LineNumber = lineNumber;
                result.Lines.Add(line);
                lineNumber++;
            }

            return result;
        } // method ParseStream

        /// <summary>
        /// Parse local file.
        /// </summary>
        public static FstFile ParseLocalFile
            (
                string fileName,
                Encoding encoding
            )
        {
            using TextReader reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    encoding
                );
            var result = ParseStream(reader);
            result.FileName = Path.GetFileName(fileName);

            return result;
        } // method ParseLocalFile

        /// <summary>
        /// Should serialize the <see cref="Lines"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeLines()
        {
            return Lines.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            reader.ReadCollection(Lines);
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            /*

            writer.WriteNullable(FileName);
            writer.WriteCollection(Lines);

            */
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<FstFile>(this, throwOnError);

            verifier.Assert(Lines.Count != 0, "Lines.Count != 0");
            foreach (var line in Lines)
            {
                verifier.VerifySubObject(line);
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => FileName.ToVisibleString();

        #endregion

    } // class FstFile

} // namespace ManagedIrbis.Fst
