// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TermInfoEx.cs -- extended term info
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Extended search term info.
    /// </summary>
    [XmlRoot("term")]
    [DebuggerDisplay("[{Count}] {Text} {Formatted}")]
    public sealed class TermInfoEx
        : Term
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonPropertyName("tag")]
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [XmlAttribute("occurrence")]
        [JsonPropertyName("occurrence")]
        public int Occurrence { get; set; }

        /// <summary>
        /// Смещение от начала поля.
        /// </summary>
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Расформатированная запись
        /// </summary>
        [XmlAttribute("formatted")]
        [JsonPropertyName("formatted")]
        public string? Formatted { get; set; }

        #endregion

        #region Private members

        private static char[] _separators = { '\x1E' };

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static TermInfoEx[] ParseEx
            (
                Response response
            )
        {
            var result = new List<TermInfoEx>();

            Regex regex = new Regex(@"^(\d+)\#(\d+)#(\d+)#(\d+)#(\d+)$");
            while (true)
            {
                string line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                string[] parts = line.Split(_separators, 3);
                if (parts.Length != 3)
                {
                    Magna.Error
                        (
                            "TermInfoEx::ParseEx: "
                            + "bad format: "
                            + line.ToVisibleString()
                        );

                    throw new IrbisNetworkException();
                }

                Match match = regex.Match(parts[0]);
                if (!match.Success)
                {
                    Magna.Error
                        (
                            "TermInfoEx::ParseEx: "
                            + "bad format: "
                            + parts[0].ToVisibleString()
                        );

                    throw new IrbisNetworkException();
                }

                TermInfoEx item = new TermInfoEx
                    {
                        Count = int.Parse(match.Groups[1].Value),
                        Mfn = int.Parse(match.Groups[2].Value),
                        Tag = int.Parse(match.Groups[3].Value),
                        Occurrence = int.Parse(match.Groups[4].Value),
                        Index = int.Parse(match.Groups[5].Value),
                        Text = parts[1],
                        Formatted = parts[2]
                    };
                result.Add(item);
            }


            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Tag"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeTag()
        {
            return Tag != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Occurrence"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeOccurrence()
        {
            return Occurrence != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="Index"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeIndex()
        {
            return Index != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public override void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            base.RestoreFromStream(reader);
            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Index = reader.ReadPackedInt32();
            Formatted = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public override void SaveToStream
            (
                BinaryWriter writer
            )
        {
            base.SaveToStream(writer);

            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Index)
                .WriteNullable(Formatted);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public override bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<TermInfoEx>(this, throwOnError);

            base.Verify(throwOnError);
            verifier
                .Assert(Mfn > 0, "MFN > 0")
                .Assert(Tag != 0, "Tag != 0")
                .Assert(Occurrence > 0, "Occurrence > 0");

            return verifier.Result;

        } // method Verify

        #endregion

    } // class TermInfoEx

} // namespace ManagedIrbis.Infrastructure
