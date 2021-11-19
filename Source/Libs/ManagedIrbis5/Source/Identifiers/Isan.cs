// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Isan.cs -- ISAN
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Runtime;
using AM.Text;

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // https://en.wikipedia.org/wiki/International_Standard_Audiovisual_Number
    //
    // International Standard Audiovisual Number (ISAN) is a unique identifier
    // for audiovisual works and related versions, similar to ISBN for books.
    // It was developed within an ISO (International Organisation
    // for Standardisation) TC46/SC9 working group. ISAN is managed
    // and run by ISAN-IA.
    //
    // The ISAN standard (ISO standard 15706:2002 & ISO 15706-2)
    // is recommended or required as the audiovisual identifier
    // of choice for producers, studios, broadcasters, Internet media
    // providers and video games publishers who need to encode, track,
    // and distribute video in a variety of formats. It provides a unique,
    // internationally recognized and permanent reference number
    // for each audiovisual work and related versions registered
    // in the ISAN system.
    //
    // ISAN identifies works throughout their entire life cycle from
    // conception, to production, to distribution and consumption.
    //
    // ISANs can be incorporated in both digital and physical media,
    // such as theatrical release prints, DVDs, publications, advertising,
    // marketing materials and packaging, as well as licensing contracts
    // to uniquely identify works.
    //
    // The ISAN identifier is incorporated in many draft and final standards
    // such as AACS, DCI, MPEG, DVB, and ATSC. The identifier can be
    // provided under descriptor 13 (0x0D) for Copyright identification
    // system and reference within an ITU-T Rec. H.222
    // or ISO/IEC 13818 program.
    //
    // The ISAN is a 12 byte block comprising three segments:
    // a 6 byte root, a 2 byte episode or part, and a 4 byte version.

    /// <summary>
    /// International Standard Audiovisual Number.
    /// </summary>
    [XmlRoot ("isan")]
    public sealed class Isan
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// 6-byte root.
        /// </summary>
        [XmlElement ("root")]
        [JsonPropertyName ("root")]
        public ulong Root { get; set; }

        /// <summary>
        /// 2-byte episode or part.
        /// </summary>
        [XmlElement ("episode")]
        [JsonPropertyName ("episode")]
        public ushort Episode { get; set; }

        /// <summary>
        /// 4-byte version.
        /// </summary>
        [XmlElement ("version")]
        [JsonPropertyName ("version")]
        public uint Version { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстового представления.
        /// </summary>
        public void ParseText
            (
                ReadOnlySpan<char> text
            )
        {
            Sure.NotEmpty (text);

            // Записывается в одной из форм

            // ---- root ----   episode    check1    version    check2
            // 1234-5678-9ABC -  0123    -   0    - 0123-4567 -   0

            // или

            // ---- root ----   episode    version
            // 1234-5678-9ABC -  0123   - 0123-4567

            var navigator = new ValueTextNavigator(text);
            Root = HexLong (navigator.ReadString (4)) << 16;
            navigator.SkipChar ('-');
            Root += HexLong (navigator.ReadString (4));
            navigator.SkipChar ('-');
            Root <<= 16;
            Root += HexLong (navigator.ReadString (4));
            navigator.SkipChar ('-');
            Episode = HexShort (navigator.ReadString (4));
            navigator.SkipChar ('-');
            var peek = navigator.PeekString (4);
            if (peek.Contains ('-'))
            {
                // это была контрольная цифра
                navigator.ReadChar();
                navigator.SkipChar ('-');
            }
            Version = HexInt (navigator.ReadString (4)) << 16;
            navigator.SkipChar ('-');
            Version += HexInt (navigator.ReadString (4));
        }

        #endregion

        #region Private members

        private static ushort Hex (char c)
        {
            return (ushort)
                (
                    c is >= 'A' and <= 'F'
                        ? c - 'A' + 10
                        : c is >= 'a' and <= 'f'
                            ? c - 'a' + 10
                            : c - '0'
                );
        }

        private static ulong HexLong (ReadOnlySpan<char> span)
        {
            CheckSpan (span);

            var result = (((ulong) Hex (span[0])) << 12)
                         + (((ulong) Hex (span[1])) << 8)
                         + (((ulong) Hex (span[2])) << 4)
                         + Hex (span[3]);

            return result;
        }

        private static ushort HexShort (ReadOnlySpan<char> span)
        {
            CheckSpan (span);

            var result = (ushort) ((Hex (span[0]) << 12)
                         + (Hex (span[1]) << 8)
                         + (Hex (span[2]) << 4)
                         + Hex (span[3]));

            return result;
        }

        private static readonly char[] goodChars = "0123456789ABCDEFabcdefg".ToCharArray();

        private static void CheckSpan (ReadOnlySpan<char> span)
        {
            if (span.Length != 4)
            {
                throw new FormatException();
            }

            foreach (var c in span)
            {
                if (!goodChars.Contains (c))
                {
                    throw new FormatException();
                }
            }
        }

        private static uint HexInt (ReadOnlySpan<char> span)
        {
            CheckSpan (span);

            var result = (uint) ((Hex (span[0]) << 12)
                         + (Hex (span[1]) << 8)
                         + (Hex (span[2]) << 4)
                         + Hex (span[3]));

            return result;
        }

        private static string Hex (ulong value)
        {
            return (value >> 32).ToString ("X4")
                   + "-"
                   + ((value >> 16) & 0xFFFFU).ToString ("X4")
                   + "-"
                   + (value & 0xFFFFU).ToString ("X4");
        }

        private static string Hex (uint value)
        {
            return (value >> 16).ToString ("X4")
                   + "-"
                   + (value & 0xFFFFU).ToString ("X4");
        }

        private static string Hex (ushort value)
        {
            return value.ToString ("X4");
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Root = reader.ReadUInt64();
            Episode = reader.ReadUInt16();
            Version = reader.ReadUInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.Write (Root);
            writer.Write (Episode);
            writer.Write (Version);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Isan> (this, throwOnError);

            verifier
                .Assert (Root != 0)
                .Assert (Version != 0);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Hex (Root)}-{Hex (Episode)}-{Hex (Version)}";
        }

        #endregion
    }
}
