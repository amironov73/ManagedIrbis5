// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MorphologyEntry.cs -- entry of the morphology database
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
    /// Entry of the morphology database.
    /// </summary>
    [XmlRoot("word")]
    public sealed class MorphologyEntry
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
        /// Main term. Field 10.
        /// </summary>
        [Field(10)]
        [XmlAttribute("main")]
        [JsonPropertyName("main")]
        public string? MainTerm { get; set; }

        /// <summary>
        /// Dictionary term. Field 11.
        /// </summary>
        [Field(11)]
        [XmlAttribute("dictionary")]
        [JsonPropertyName("dictionary")]
        public string? Dictionary { get; set; }

        /// <summary>
        /// Language name. Field 12.
        /// </summary>
        [Field(12)]
        [XmlAttribute("language")]
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Forms of the word. Repeatable field 20.
        /// </summary>
        [Field(20)]
        [XmlElement("form")]
        [JsonPropertyName("forms")]
        public string[]? Forms { get; set; }

        #endregion

        #region Private members

        private static void _AddField
            (
                Record record,
                int tag,
                string? text
            )
        {
            if (!text.IsEmpty())
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

            _AddField(result, 10, MainTerm);
            _AddField(result, 11, Dictionary);
            if (!ReferenceEquals(Forms, null))
            {
                foreach (string synonym in Forms)
                {
                    _AddField(result, 20, synonym);
                }
            }
            _AddField(result, 12, Language);

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static MorphologyEntry Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new MorphologyEntry
            {
                Mfn = record.Mfn,
                MainTerm = record.FM(10),
                Dictionary = record.FM(11),
                Language = record.FM(12),
                Forms = record.FMA(20)
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            MainTerm = reader.ReadNullableString();
            Dictionary = reader.ReadNullableString();
            Language = reader.ReadNullableString();
            Forms = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(MainTerm)
                .WriteNullable(Dictionary)
                .WriteNullable(Language)
                .WriteNullableArray(Forms);
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
                = new Verifier<MorphologyEntry>(this, throwOnError);

            verifier
                .NotNullNorEmpty(MainTerm, "MainTerm")
                .NotNullNorEmpty(Dictionary, "Dictionary")
                .NotNullNorEmpty(Language, "Language")
                .NotNull(Forms, "Forms");

            if (!ReferenceEquals(Forms, null))
            {
                foreach (string form in Forms)
                {
                    verifier.NotNullNorEmpty(form, "form");
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

            result.AppendFormat("{0}: ", MainTerm.ToVisibleString());
            if (!ReferenceEquals(Forms, null)
                && Forms.Length != 0)
            {
                result.Append(string.Join(", ", Forms));
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
