// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TermPosting.cs -- постинг термина
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
    /// Постинг термина.
    /// </summary>
    [XmlRoot("posting")]
    [DebuggerDisplay("[{Mfn}] {Tag} {Occurrence} {Count} {Text}")]
    public sealed class TermPosting
        : IEquatable<TermPosting>,
        IHandmadeSerializable,
        IVerifiable
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
        /// Количество повторений.
        /// </summary>
        [XmlAttribute("count")]
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Результат форматирования.
        /// </summary>
        [XmlAttribute("text")]
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="TermPosting"/>.
        /// </summary>
        public TermPosting Clone()
        {
            return (TermPosting) MemberwiseClone();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static TermPosting[] Parse
            (
                Response response
            )
        {
            var result = new List<TermPosting>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split('#', 5);
                if (parts.Length < 4)
                {
                    break;
                }

                var item = new TermPosting
                {
                    Mfn = int.Parse(parts[0]),
                    Tag = int.Parse(parts[1]),
                    Occurrence = int.Parse(parts[2]),
                    Count = int.Parse(parts[3]),
                    Text = parts.Length == 5 ? parts[4] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
        public bool Equals(TermPosting? other)
        {
            if (other is not null)
            {
                return Mfn == other.Mfn
                       && Tag == other.Tag
                       && Occurrence == other.Occurrence
                       && Count == other.Count
                       && Text == other.Text;
            }

            return false;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Tag = reader.ReadPackedInt32();
            Occurrence = reader.ReadPackedInt32();
            Count = reader.ReadPackedInt32();
            Text = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32(Tag)
                .WritePackedInt32(Occurrence)
                .WritePackedInt32(Count)
                .WriteNullable(Text);
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
            var verifier = new Verifier<TermPosting>(this, throwOnError);

            verifier
                .Assert(Mfn > 0, "Mfn")
                .Assert(Tag > 0, "Tag")
                .Assert(Occurrence > 0, "Occurrence")
                .Assert(Count > 0, "Count");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return $"{Mfn}#{Tag}#{Occurrence}#{Count}#{Text}";
        }

        #endregion
    }
}
