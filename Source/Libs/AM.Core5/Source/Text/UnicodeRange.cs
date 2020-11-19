// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UnicodeRange.cs -- диапазон символов Unicode
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Диапазон символов Unicode.
    /// </summary>
    [XmlRoot("range")]
    public sealed class UnicodeRange
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Static

        /// <summary>
        /// Control characters.
        /// </summary>
        public static readonly UnicodeRange ControlCharacters
            = new UnicodeRange("Control characters", '\x0000', '\x001F');

        /// <summary>
        /// Basic Latin.
        /// </summary>
        public static readonly UnicodeRange BasicLatin
            = new UnicodeRange("Basic Latin", '\x0020', '\x007F');

        /// <summary>
        /// Latin1 supplement.
        /// </summary>
        public static readonly UnicodeRange Latin1Supplement
            = new UnicodeRange("Latin Supplement", '\x0080', '\x00FF');

        /// <summary>
        /// Latin extended.
        /// </summary>
        public static readonly UnicodeRange LatinExtended
            = new UnicodeRange("Latin Extended", '\x0100', '\x024F');

        /// <summary>
        /// Cyrillic.
        /// </summary>
        public static readonly UnicodeRange Cyrillic
            = new UnicodeRange("Cyrillic", '\x0400', '\x04FF');

        /// <summary>
        /// Cyrillic supplement.
        /// </summary>
        public static readonly UnicodeRange CyrillicSupplement
            = new UnicodeRange("Cyrillic Supplement", '\x0500', '\x052F');

        /// <summary>
        /// Russian.
        /// </summary>
        public static readonly UnicodeRange Russian
            = new UnicodeRange("Russian", '\x0410', '\x0451');

        #endregion

        #region Properties

        /// <summary>
        /// Name.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// From.
        /// </summary>
        [XmlAttribute("from")]
        [JsonPropertyName("from")]
        public char From { get; set; }

        /// <summary>
        /// To (including).
        /// </summary>
        [XmlAttribute("to")]
        [JsonPropertyName("to")]
        public char To { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnicodeRange()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnicodeRange
            (
                string name,
                char fromChar,
                char toChar
            )
        {
            Sure.NotNull(name, nameof(name));

            if (fromChar > toChar)
            {
                throw new ArgumentException();
            }

            Name = name;
            From = fromChar;
            To = toChar;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            Name = reader.ReadNullableString();
            From = reader.ReadChar();
            To = reader.ReadChar();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.WriteNullable(Name);
            writer.Write(From);
            writer.Write(To);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify(bool throwOnError)
        {
            var verifier = new Verifier<UnicodeRange>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .Assert(From <= To, "From <= To");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString()
                   + ": "
                   + ((int)From).ToInvariantString()
                   + "-"
                   + ((int)To).ToInvariantString();
        }

        #endregion
    }
}
