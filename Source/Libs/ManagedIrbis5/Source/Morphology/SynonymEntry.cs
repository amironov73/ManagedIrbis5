// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SynonymEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Morphology
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("word")]
    public class SynonymEntry
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Record MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Main word. Field 10.
        /// </summary>
        [Field(10)]
        [XmlAttribute("main")]
        [JsonPropertyName("main")]
        public string? MainWord { get; set; }

        /// <summary>
        /// Synonyms. Field 11.
        /// </summary>
        [Field(11)]
        [XmlElement("synonym")]
        [JsonPropertyName("synonyms")]
        public string[]? Synonyms { get; set; }

        /// <summary>
        /// Language code. Field 12.
        /// </summary>
        [Field(12)]
        [XmlAttribute("language")]
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Worksheet code. Field 920.
        /// </summary>
        [Field(920)]
        [XmlAttribute("worksheet")]
        [JsonPropertyName("worksheet")]
        public string? Worksheet { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                Record record,
                int tag,
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                record.Fields.Add(new Field { Tag = tag, Value = text });
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode the record.
        /// </summary>
        public Record Encode()
        {
            var result = new Record
            {
                Mfn = Mfn
            };

            _AddField(result, 10, MainWord.ThrowIfNull(nameof(MainWord)));
            if (!ReferenceEquals(Synonyms, null))
            {
                foreach (string synonym in Synonyms)
                {
                    _AddField(result, 11, synonym);
                }
            }
            _AddField(result, 12, Language);
            _AddField(result, 920, Worksheet);

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static SynonymEntry Parse
            (
                Record record
            )
        {

            var result = new SynonymEntry
            {
                Mfn = record.Mfn,
                MainWord = record.FM(10),
                Synonyms = record.FMA(11),
                Language = record.FM(12),
                Worksheet = record.FM(920)
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn() => Mfn != 0;

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            MainWord = reader.ReadNullableString();
            Synonyms = reader.ReadNullableStringArray();
            Language = reader.ReadNullableString();
            Worksheet = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(MainWord)
                .WriteNullableArray(Synonyms)
                .WriteNullable(Language)
                .WriteNullable(Worksheet);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<SynonymEntry>(this, throwOnError);

            verifier
                .NotNullNorEmpty(MainWord, "MainWord")
                .NotNullNorEmpty(Worksheet, "Worksheet")
                .NotNullNorEmpty(Language, "Language")
                .NotNull(Synonyms, "Synonyms");

            if (!ReferenceEquals(Synonyms, null))
            {
                foreach (string synonym in Synonyms)
                {
                    verifier.NotNullNorEmpty(synonym, "synonym");
                }
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("{0}: ", MainWord.ToVisibleString());
            if (!ReferenceEquals(Synonyms, null)
                && Synonyms.Length != 0)
            {
                result.Append(string.Join(", ", Synonyms));
            }
            else
            {
                result.Append("(none)");
            }

            return result.ToString();
        }

        #endregion
    }
}
