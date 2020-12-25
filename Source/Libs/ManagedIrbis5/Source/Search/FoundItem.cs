// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FoundLine.cs -- одна строка в ответе на поисковый запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Одна строка в ответе сервера на поисковый запрос.
    /// </summary>
    [XmlRoot("item")]
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
        : IEquatable<FoundItem>,
        IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Text.
        /// </summary>
        [XmlAttribute("text")]
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static FoundItem[] Parse
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<FoundItem>(expected);
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var item = new FoundItem
                {
                    Mfn = int.Parse(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static int[] ParseMfn
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<int>(expected);
            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 2);
                var mfn = int.Parse(parts[0]);
                result.Add(mfn);
            }

            return result.ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WriteNullable(Text);
        }

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T?)" />
        public bool Equals(FoundItem? other)
            => Mfn == other?.Mfn;

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<FoundItem>(this, throwOnError);

            verifier
                .Assert(Mfn > 0, "Mfn > 0")
                .NotNullNorEmpty(Text, "Text");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"[{Mfn}] {Text.ToVisibleString()}";
        }

        #endregion

    }
}
